using UnityEngine;

namespace Game
{
    public class Ladder : MonoBehaviour
    {
        public float MaxHeight;

        public void Awake()
        {
            var bounds = GetComponent<Collider2D>().bounds;
            MaxHeight = bounds.max.y;
        }
    }
}