using System;
using UnityEngine;

namespace Game
{
    public class Room1Leaf : MonoBehaviour
    {
        public event Action Changed;

        public bool IsRight { get; private set; }

        public int Position;

        public bool InitialPosition;

        private SpriteRenderer sprite;
        private Animator leafAnimator;

        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            leafAnimator = GetComponentInChildren<Animator>();

            leafAnimator.SetBool("IsRight", IsRight);

            if (InitialPosition)
                Switch();
        }

        public void Switch()
        {
            sprite.transform.localPosition = Vector3.Scale(sprite.transform.localPosition, Vector3.left);
            IsRight = !IsRight;

            Changed?.Invoke();

            leafAnimator.SetBool("IsRight", IsRight);
        }
    }
}