using System.Collections;
using UnityEngine;

namespace Game
{
    public class Room2LightCharacter : MonoBehaviour
    {
        public SpriteRenderer Light;

        public Color Color;

        private void Start()
        {
            LightOff();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                Room2Controller.Instance.Mark(Color);
            }
        }

        public void LightOn()
        {
            Light.color = Color;
        }

        public void LightOff()
        {
            Light.color = new Color(Color.r, Color.g, Color.b, 0.2f);
        }

        public void FlashOnce()
        {
            StartCoroutine(FlashOnceCoroutine());
        }

        private IEnumerator FlashOnceCoroutine()
        {
            LightOn();
            yield return new WaitForSeconds(0.5f);
            LightOff();
        }
    }
}