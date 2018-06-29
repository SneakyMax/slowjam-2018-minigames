using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Room3HiddenPiece : MonoBehaviour
    {
        public int PieceNum;

        public SpriteRenderer ToRemove;

        private bool isRemoved;

        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
        }

        private void OnInteract()
        {
            if (PlayerController.Instance.Holding != null || isRemoved)
                return;

            isRemoved = true;

            if (ToRemove != null)
                ToRemove.DOFade(0, 0.5f);
        }
    }
}