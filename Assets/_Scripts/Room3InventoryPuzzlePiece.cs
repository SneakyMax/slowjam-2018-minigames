using UnityEngine;

namespace Game
{
    public class Room3InventoryPuzzlePiece : MonoBehaviour
    {
        [Range(0, 3)]
        public int Index;

        public SpriteRenderer SpriteRenderer;

        [Range(0, 3)]
        public int Rotation;

        private void Awake()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Rotation = Random.Range(0, 4);
        }
    }
}