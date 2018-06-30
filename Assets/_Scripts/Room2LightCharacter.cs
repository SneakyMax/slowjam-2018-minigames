using System.Collections;
using System.Linq;
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponentInParent<PlayerController>() && other.contacts.Any(x => Vector2.Dot(Vector2.up, x.normal) < -0.95))
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