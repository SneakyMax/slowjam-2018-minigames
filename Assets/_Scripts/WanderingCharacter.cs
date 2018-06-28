using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class WanderingCharacter : MonoBehaviour
    {
        private enum WanderingState
        {
            MovingLeft,
            Still,
            MovingRight,
            Hop
        }

        private Rigidbody2D body;

        private float newStateAt;

        public float MinTimeBetweenChanges;
        public float MaxTimeBetweenChanges;

        private WanderingState currentState;

        public float MoveSpeed = 1;

        public float HopForce = 1;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            foreach (var playerCollider in PlayerController.Instance.Colliders)
            {
                // Ignore collisions between the MAIN collider for the character, and the player's collider(s)
                // this will still allow the other platform collider to work.
                Physics2D.IgnoreCollision(playerCollider, GetComponent<Collider2D>());
            }
        }

        private void FixedUpdate()
        {
            if (Time.time > newStateAt)
            {
                // Go other way for some amount of time.
                newStateAt = Time.time + Random.Range(MinTimeBetweenChanges, MaxTimeBetweenChanges);

                var previousState = currentState;
                while(previousState == currentState)
                    currentState = Enum.GetValues(typeof(WanderingState)).Cast<WanderingState>().ElementAt(Random.Range(0, Enum.GetNames(typeof(WanderingState)).Length));

                if (currentState == WanderingState.Hop)
                {
                    body.velocity = new Vector2();
                    body.AddForce(Vector2.up * HopForce, ForceMode2D.Impulse);
                }
            }

            switch (currentState)
            {
                case WanderingState.MovingLeft:
                    body.velocity = new Vector2(-MoveSpeed, body.velocity.y);
                    break;
                case WanderingState.MovingRight:
                    body.velocity = new Vector2(MoveSpeed, body.velocity.y);
                    break;
                case WanderingState.Still:
                    body.velocity = new Vector2(0, body.velocity.y);
                    break;
            }
        }
    }
}