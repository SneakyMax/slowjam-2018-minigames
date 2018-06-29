using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Room3PuzzlePlacer : MonoBehaviour
    {
        public event Action Changed;

        public SpriteRenderer Target;

        public float RotationTime;

        [Range(0, 3)]
        public int CorrectPiece;

        [Range(0, 3)]
        public int CorrectRotation;

        private GameObject item;
        private int currentIndex;

        private int rotation;

        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
        }

        private void OnInteract()
        {
            if (Room3Controller.Instance.Solved)
                return;

            var previousItem = item;
            var playerHasItem = PlayerController.Instance.Holding != null;

            if (playerHasItem)
            {
                var piece = PlayerController.Instance.Holding.GetComponent<Room3InventoryPuzzlePiece>();
                if (piece == null)
                    return; // Has something else, can't do anything

                item = PlayerController.Instance.TakeItem();
                item.SetActive(false);
                Target.sprite = piece.SpriteRenderer.sprite;
                currentIndex = piece.Index;
                rotation = piece.Rotation;
                Target.transform.localEulerAngles = new Vector3(Target.transform.localRotation.x, Target.transform.localRotation.y, rotation * 90);
            }

            if (previousItem != null)
            {
                PlayerController.Instance.GiveItem(previousItem);

                if (!playerHasItem)
                {
                    Target.sprite = null;
                    currentIndex = -1;
                    rotation = 0;
                    item = null;
                }
            }

            Changed?.Invoke();
        }

        public void Rotate()
        {
            if (item == null)
                return;

            var currentRotation = rotation;
            var newRotation = currentRotation + 1;

            Target.gameObject.transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 90), RotationTime, RotateMode.FastBeyond360);

            rotation = newRotation;

            item.GetComponent<Room3InventoryPuzzlePiece>().Rotation = rotation;

            Changed?.Invoke();
        }

        public bool Check()
        {
            if (!item)
                return false;
            return rotation % 4 == CorrectRotation && item.GetComponent<Room3InventoryPuzzlePiece>().Index == CorrectPiece;
        }
    }
}