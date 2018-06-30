using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public enum ControlledState
        {
            Idle,
            MoveLeft,
            MoveRight
        }

        public static PlayerController Instance { get; private set; }

        public float MoveSpeed = 4;

        public float JumpForce = 3;

        public float LadderVerticalSpeed = 2;

        public float LadderHorizontalSpeed = 1;

        public SpriteRenderer ArrowUp;
        public SpriteRenderer ExclamationPoint;

        private bool isInAir;
        private float lastJumpTime;
        private Ladder currentLadder;
        private Ladder currentLadderCan;

        private float maxY;

        public IList<Collider2D> Colliders { get; private set; }
        private IList<Interactable> interactables;
        private Animator animator;
        private bool fallingOffLadder;
        private bool immediatelyGrabLadder;
        
        public bool CannotTransition { get; set; }
        
        public Rigidbody2D Body { get; private set; }

        private float previousVerticalInput;

        public GameObject Holding;

        public Transform HoldItemPosition;

        public bool IsControlled { get; set; }

        public ControlledState ForceMovement { get; set; }

        private void Awake()
        {
            Instance = this;

            Body = GetComponent<Rigidbody2D>();

            Colliders = GetComponentsInChildren<Collider2D>();
            interactables = new List<Interactable>();
            animator = GetComponentInChildren<Animator>();
            ForceMovement = ControlledState.Idle;
        }

        private void Start()
        {
            animator.SetBool("IsOnGround", true);
        }

        private void Update()
        {
            var isTryingToGetOnLadder = InputManager.Vertical > 0.5 || (InputManager.Vertical < -0.5 && previousVerticalInput >= -0.5);
            if ( (isTryingToGetOnLadder || immediatelyGrabLadder) && currentLadderCan != null)
            {
                LadderOn();
                immediatelyGrabLadder = false;
            }

            //maxY = colliders.Max(x => x.bounds.max.y);
            maxY = transform.position.y;

            ArrowUp.gameObject.SetActive(currentLadderCan != null && currentLadder == null);
            ExclamationPoint.gameObject.SetActive(interactables.Count > 0);

            TryInteract();

            previousVerticalInput = InputManager.Vertical;

            for (var i = interactables.Count - 1; i >= 0; i--)
            {
                if (interactables[i].Can == false || !interactables[i].isActiveAndEnabled)
                {
                    interactables.RemoveAt(i);
                }
            }
        }

        private void TryInteract()
        {
            if (InputManager.Interact && interactables.Count > 0)
            {
                var closest = interactables.OrderBy(x => x.Collider.Distance(Colliders.First()).distance).First();
                closest.Interact();
                animator.SetTrigger("Poke");
            }
        }

        public void FixedUpdate()
        {
            if (IsControlled)
            {
                ControlledPhysics();
            }
            else if (currentLadder != null)
            {
                LadderPhysics();
            }
            else
            {
                PlatformerPhysics();
            }
        }

        private void ControlledPhysics()
        {
            switch (ForceMovement)
            {
                case ControlledState.MoveLeft:
                    Body.velocity = new Vector2(-MoveSpeed, Body.velocity.y);
                    animator.SetInteger("Moving", -1);
                    break;
                case ControlledState.MoveRight:
                    Body.velocity = new Vector2(MoveSpeed, Body.velocity.y);
                    animator.SetInteger("Moving", 1);
                    break;
                case ControlledState.Idle:
                    Body.velocity = new Vector2(0, Body.velocity.y);
                    animator.SetInteger("Moving", 0);
                    break; 
            }
        }

        private void PlatformerPhysics()
        {
            Body.gravityScale = 1;

            Body.AddForce(Vector2.right * InputManager.Horizontal * MoveSpeed * 10, ForceMode2D.Force);

            if (Body.velocity.x > MoveSpeed || Body.velocity.x < -MoveSpeed)
            {
                Body.velocity = new Vector2(Mathf.Sign(Body.velocity.x) * MoveSpeed, Body.velocity.y);
            }

            if (Mathf.Abs(InputManager.Horizontal) < 0.1)
            {
                animator.SetInteger("WalkDirection", 0);
            }
            else
            {
                animator.SetInteger("WalkDirection", (int)Mathf.Sign(InputManager.Horizontal));
            }

            if (Mathf.Abs(InputManager.Horizontal) < 0.1)
            {
                Body.velocity = new Vector2(
                    Mathf.Sign(Body.velocity.x) * Mathf.Min(1, Mathf.Abs(Body.velocity.x)),
                    Body.velocity.y);
            }

            if (InputManager.Jump && !isInAir)
            {
                isInAir = true;
                Body.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                lastJumpTime = Time.time;
                animator.SetTrigger("Jump");
                animator.SetBool("IsOnGround", false);
            }
        }

        private void LadderOn()
        {
            currentLadder = currentLadderCan;
            if (currentLadder.AttachedPlatform != null)
                currentLadder.DisablePlatform();
        }

        private void LadderOff()
        {
            if (currentLadder.AttachedPlatform != null)
                currentLadder.EnablePlatform();
            currentLadder = null;
        }

        private void LadderPhysics()
        {
            if (currentLadderCan == null)
            {
                LadderOff();
                return;
            }

            Body.gravityScale = 0;

            Body.AddForce(Vector2.right * InputManager.Horizontal * LadderHorizontalSpeed * 10, ForceMode2D.Force);

            if (maxY < currentLadder.MaxHeight || InputManager.Vertical < 0)
            {
                Body.AddForce(Vector2.up * InputManager.Vertical * LadderVerticalSpeed * 10, ForceMode2D.Force);
            } 
            else if (maxY >= currentLadder.MaxHeight && InputManager.Vertical > 0.5 && currentLadder.AttachedPlatform != null)
            {
                Body.velocity = new Vector2(Body.velocity.x, 0);
                fallingOffLadder = true;
                currentLadderCan = null;
                LadderOff();
                return;
            }

            if (Body.velocity.x > LadderHorizontalSpeed || Body.velocity.x < -LadderHorizontalSpeed)
            {
                Body.velocity = new Vector2(Mathf.Sign(Body.velocity.x) * LadderHorizontalSpeed, Body.velocity.y);
            }

            if (Body.velocity.y > LadderVerticalSpeed || Body.velocity.y < -LadderVerticalSpeed)
            {
                Body.velocity = new Vector2(Body.velocity.x, Mathf.Sign(Body.velocity.y) * LadderVerticalSpeed);
            }

            if (maxY >= currentLadder.MaxHeight && InputManager.Vertical >= 0)
                Body.velocity = new Vector2(Body.velocity.x, 0);

            if (Mathf.Abs(InputManager.Horizontal) < 0.1)
            {
                Body.velocity = new Vector2(0, Body.velocity.y);
            }

            if (Mathf.Abs(InputManager.Vertical) < 0.1)
            {
                Body.velocity = new Vector2(Body.velocity.x, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Enter area of ladder
            if (other.CompareTag("Ladder") && !fallingOffLadder)
            {
                var ladder = other.GetComponent<Ladder>();
                if (maxY < ladder.MaxHeight)
                    currentLadderCan = ladder;
            }

            // Enter area of interactable
            var interactable = other.GetComponentInParent<Interactable>();
            if (interactable != null && interactable.Can)
            {
                interactables.Add(interactable);
            }

            if (other.GetComponent<ZoomZone>())
            {
                CameraController.Instance.SetOrthoSize(other.GetComponent<ZoomZone>().TargetSize);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (currentLadderCan != null)
                return;

            // Enter area of ladder
            if (other.CompareTag("Ladder") && !fallingOffLadder)
            {
                var ladder = other.GetComponent<Ladder>();
                if (maxY < ladder.MaxHeight)
                    currentLadderCan = ladder;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Lead area of a ladder
            if (other.CompareTag("Ladder") && currentLadderCan == other.GetComponent<Ladder>())
            {
                currentLadderCan = null;
            }

            // Leave area of an interactable
            var interactable = other.GetComponentInParent<Interactable>();
            if (interactable != null && interactables.Contains(interactable))
            {
                interactables.Remove(interactable);
            }

            if (other.GetComponent<ZoomZone>())
            {
                CameraController.Instance.UnsetOrthoSize();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.enabled)
                return;

            var contacts = collision.contacts;
            foreach(var contact in contacts)
            {
                // Checking if you've landed on the ground.
                if (Mathf.Abs(Vector2.Dot(contact.normal, Vector2.up)) > 0.9 && Time.time - lastJumpTime > 0.5)
                {
                    isInAir = false;
                    animator.SetBool("IsOnGround", true);
                    fallingOffLadder = false;
                }

                // Disembarking from ladder at bottom.
                if (currentLadder != null && InputManager.Vertical < -0.5)
                {
                    LadderOff();
                }
            }
        }

        public void ImmediatelyGrabLadder()
        {
            immediatelyGrabLadder = true;
        }

        public GameObject TakeItem()
        {
            var item = Holding;
            Holding = null;
            item.transform.parent = null;
            return item;
        }

        public void GiveItem(GameObject currentItem)
        {
            if (Holding != null)
                throw new InvalidOperationException("Already holding item - check for holding item first");

            Holding = currentItem;
            Holding.transform.parent = transform;
            Holding.SetActive(true);
            Holding.transform.position = HoldItemPosition.position;

            var puzzlePiece = Holding.GetComponent<Room3InventoryPuzzlePiece>();
            if (puzzlePiece != null)
            {
                Holding.transform.localEulerAngles = new Vector3(Holding.transform.localEulerAngles.x, Holding.transform.localEulerAngles.y, puzzlePiece.Rotation * 90);
            }
            else
            {
                Holding.transform.localEulerAngles = new Vector3();
            }
        }

        private IEnumerator CanTransitionAfterDelay()
        {
            yield return new WaitForSeconds(2);
            CannotTransition = false;
        }

        public void DelayTransitonable()
        {
            StartCoroutine(CanTransitionAfterDelay());
        }
    }
}
