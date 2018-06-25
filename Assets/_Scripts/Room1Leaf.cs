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

        private void Awake()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();

            if (InitialPosition)
                Switch();
        }

        public void Switch()
        {
            // TODO animation
            sprite.transform.localPosition = Vector3.Scale(sprite.transform.localPosition, Vector3.left);
            IsRight = !IsRight;

            Changed?.Invoke();
        }
    }
}