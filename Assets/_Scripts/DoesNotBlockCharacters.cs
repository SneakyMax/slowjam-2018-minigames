using UnityEngine;

namespace Game
{
    public class DoesNotBlockCharacters : MonoBehaviour
    {
        private void Start()
        {
            var myCollider = GetComponentInChildren<Collider2D>();
            var characters = GameObject.FindGameObjectsWithTag("Character");
            foreach (var character in characters)
            {
                foreach (var charCollider in character.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(charCollider, myCollider);
                }
            }
        }
    }
}