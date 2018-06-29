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
                Physics2D.IgnoreCollision(character.GetComponent<Collider2D>(), myCollider);
            }
        }
    }
}