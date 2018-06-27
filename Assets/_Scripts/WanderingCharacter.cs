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
            MovingRight
        }

        private Rigidbody2D body;

        private float newStateAt;

        public float MinTimeBetweenChanges;
        public float MaxTimeBetweenChanges;

        private WanderingState currentState;

        public float MoveSpeed = 1;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Time.time > newStateAt)
            {
                // Go other way for some amount of time.
                newStateAt = Time.time + Random.Range(MinTimeBetweenChanges, MaxTimeBetweenChanges);
                currentState = Enum.GetValues(typeof(WanderingState)).Cast<WanderingState>().ElementAt(Random.Range(0, Enum.GetNames(typeof(WanderingState)).Length));
            }

            switch (currentState)
            {
                case WanderingState.MovingLeft:
                    body.velocity = Vector2.left * MoveSpeed;
                    break;
                case WanderingState.MovingRight:
                    body.velocity = Vector2.right * MoveSpeed;
                    break;
                default:
                    body.velocity = new Vector2();
                    break;
            }
        }
    }
}