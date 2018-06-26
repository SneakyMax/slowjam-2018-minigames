using UnityEngine;

namespace Game
{
    public class Ladder : MonoBehaviour
    {
        public float MaxHeight;

        public PlatformEffector2D AttachedPlatform;

        private Collider2D attachedCollider;

        public void Awake()
        {
            var bounds = GetComponent<Collider2D>().bounds;
            MaxHeight = bounds.max.y;

            if (AttachedPlatform != null)
                attachedCollider = AttachedPlatform.GetComponent<Collider2D>();
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