using UnityEngine;

namespace Game
{
    public class Ladder : MonoBehaviour
    {
        public float MaxHeight;
        public float OriginalMaxHeight;

        public PlatformEffector2D AttachedPlatform;

        private Collider2D attachedCollider;

        private Collider2D myCollider;

        public void Awake()
        {
            myCollider = GetComponent<Collider2D>();
            GetMaxHeight();
            if (AttachedPlatform != null)
                attachedCollider = AttachedPlatform.GetComponent<Collider2D>();
        }

        private void GetMaxHeight()
        {
            MaxHeight = myCollider.bounds.max.y;
            OriginalMaxHeight = MaxHeight;
        }

        private void Update()
        {
            // Moving Ladders
            if (Mathf.Abs(MaxHeight - OriginalMaxHeight) < 0.01f)
            {
                GetMaxHeight();
            }
        }

        public void DisablePlatform()
        {
            foreach (var playerCollider in PlayerController.Instance.Colliders)
            {
                Physics2D.IgnoreCollision(attachedCollider, playerCollider);
            }
        }

        public void EnablePlatform()
        {
            foreach (var playerCollider in PlayerController.Instance.Colliders)
            {
                Physics2D.IgnoreCollision(attachedCollider, playerCollider, false);
            }
        }
    }
}