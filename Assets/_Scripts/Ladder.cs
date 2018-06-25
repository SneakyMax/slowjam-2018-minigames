using UnityEngine;

namespace Game
{
    public class Ladder : MonoBehaviour
    {
        public float MaxHeight { get; set; }

        public void Start()
        {
            var bounds = GetComponent<Collider2D>().bounds;
            MaxHeight = bounds.max.y;
        }
    }
}