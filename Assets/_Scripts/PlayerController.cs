using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public float MoveSpeed = 4;

        public float JumpForce = 3;

        public float LadderVerticalSpeed = 2;

        public float LadderHorizontalSpeed = 1;

        public SpriteRenderer ArrowUp;
        public SpriteRenderer ExclamationPoint;

        private Rigidbody2D body;

        private bool isInAir;
        private float lastJumpTime;
        private Ladder currentLadder;
        private Ladder currentLadderCan;

        private float maxY;

        private IList<Collider2D> colliders;
        private IList<Interactable> interactables;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();

            colliders = GetComponentsInChildren<Collider2D>();
            interactables = new List<Interactable>();
        }

        private void Update()
        {
            if (InputManager.Vertical > 0.5 && currentLadderCan != null)
            {
                currentLadder = currentLadderCan;
            }

            //maxY = colliders.Max(x => x.bounds.max.y);
            maxY = transform.position.y;

            ArrowUp.gameObject.SetActive(currentLadderCan != null && currentLadder == null);
            ExclamationPoint.gameObject.SetActive(interactables.Count > 0);

            TryInteract();
        }

        private void TryInteract()
        {
            if (InputManager.Interact && interactables.Count > 0)
            {
                var closest = interactables.OrderBy(x => x.Collider.Distance(colliders.First())).First();
                closest.Interact();
            }
        }

        public void FixedUpdate()
        {
            if (currentLadder != null)
            {
                LadderPhysics();
            }
            else
            {
                PlatformerPhysics();
            }
        }

        private void LadderPhysics()
        {
            if (currentLadderCan == null)
            {
                currentLadder = null;
                return;
            }

            body.gravityScale = 0;

            body.AddForce(Vector2.right * InputManager.Horizontal * LadderHorizontalSpeed * 10, ForceMode2D.Force);

            if (maxY < currentLadder.MaxHeight || InputManager.Vertical < 0)
                body.AddForce(Vector2.up * InputManager.Vertical * LadderVerticalSpeed * 10, ForceMode2D.Force);

            if (body.velocity.x > LadderHorizontalSpeed || body.velocity.x < -LadderHorizontalSpeed)
            {
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * LadderHorizontalSpeed, body.velocity.y);
            }

            if (body.velocity.y > LadderVerticalSpeed || body.velocity.y < -LadderVerticalSpeed)
            {
                body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * LadderVerticalSpeed);
            }

            if (maxY >= currentLadder.MaxHeight && InputManager.Vertical >= 0)
                body.velocity = new Vector2(body.velocity.x, 0);

            if (Mathf.Abs(InputManager.Horizontal) < 0.1)
            {
                body.velocity = new Vector2(
                    0,
                    body.velocity.y);
            }

            if (Mathf.Abs(InputManager.Vertical) < 0.1)
            {
                body.velocity = new Vector2(
                    body.velocity.x,
                    0);

            }
        }

        private void PlatformerPhysics()
        {
            body.gravityScale = 1;

            body.AddForce(Vector2.right * InputManager.Horizontal * MoveSpeed * 10, ForceMode2D.Force);

            if (body.velocity.x > MoveSpeed || body.velocity.x < -MoveSpeed)
            {
                body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * MoveSpeed, body.velocity.y);
            }

            if (Mathf.Abs(InputManager.Horizontal) < 0.1)
            {
                body.velocity = new Vector2(
                    Mathf.Sign(body.velocity.x) * Mathf.Min(1, Mathf.Abs(body.velocity.x)),
                    body.velocity.y);
            }

            if (InputManager.Jump && !isInAir)
            {
                isInAir = true;
                body.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                lastJumpTime = Time.time;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ladder"))
            {
                var ladder = other.GetComponent<Ladder>();
                if (maxY < ladder.MaxHeight)
                    currentLadderCan = ladder;
            }

            if (other.gameObject.GetComponent<Interactable>())
            {
                interactables.Add(other.gameObject.GetComponent<Interactable>());
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (currentLadderCan != null)
                return;

            if (other.CompareTag("Ladder"))
            {
                var ladder = other.GetComponent<Ladder>();
                if (maxY < ladder.MaxHeight)
                    currentLadderCan = ladder;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ladder") && currentLadderCan == other.GetComponent<Ladder>())
            {
                currentLadderCan = null;
            }

            if (other.gameObject.GetComponent<Interactable>())
            {
                interactables.Remove(other.gameObject.GetComponent<Interactable>());
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.enabled)
                return;

            if (Time.time - lastJumpTime < 0.5)
                return;
            
            var contacts = collision.contacts;
            foreach(var contact in contacts)
            {
                if (Mathf.Abs(Vector2.Dot(contact.normal, Vector2.up)) > 0.9)
                {
                    isInAir = false;
                }

                if (currentLadder != null && InputManager.Vertical < -0.5)
                {
                    currentLadder = null;
                }
            }
        }
    }
}
