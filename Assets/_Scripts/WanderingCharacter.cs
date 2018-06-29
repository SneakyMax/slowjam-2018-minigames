﻿using System;
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
            Hop,
            Sleeping
        }

        private Rigidbody2D body;
        private Animator animator;

        public SpriteRenderer SpriteObj;

        private float newStateAt;

        public float MinTimeBetweenChanges;
        public float MaxTimeBetweenChanges;

        private WanderingState currentState;

        public float MoveSpeed = 1;

        public float HopForce = 1;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            foreach (var playerCollider in PlayerController.Instance.Colliders)
            {
                // Ignore collisions between the MAIN collider for the character, and the player's collider(s)
                // this will still allow the other platform collider to work.
                Physics2D.IgnoreCollision(playerCollider, SpriteObj.GetComponent<BoxCollider2D>());

                foreach (var circle in SpriteObj.GetComponents<CircleCollider2D>())
                {
                    Physics2D.IgnoreCollision(playerCollider, circle);
                }
            }
        }

        private void FixedUpdate()
        {
            if (currentState == WanderingState.Sleeping)
                return;

            if (Time.time > newStateAt)
            {
                // Go other way for some amount of time.
                newStateAt = Time.time + Random.Range(MinTimeBetweenChanges, MaxTimeBetweenChanges);

                var previousState = currentState;
                while(previousState == currentState || currentState == WanderingState.Sleeping)
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
                    animator.SetInteger("Moving", -1);
                    break;
                case WanderingState.MovingRight:
                    body.velocity = new Vector2(MoveSpeed, body.velocity.y);
                    animator.SetInteger("Moving", 1);
                    break;
                case WanderingState.Still:
                    body.velocity = new Vector2(0, body.velocity.y);
                    animator.SetInteger("Moving", 0);
                    break;
                case WanderingState.Hop:
                    animator.SetInteger("Moving", 0);
                    break;
                case WanderingState.Sleeping:
                    body.velocity = new Vector2(0, body.velocity.y);
                    animator.SetBool("Sleeping", true);
                    break;       
            }
        }

        public void Sleep()
        {
            currentState = WanderingState.Sleeping;
        }
    }
}