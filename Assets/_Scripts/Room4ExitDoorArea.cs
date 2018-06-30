using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Room4ExitDoorArea : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
        }

        private void OnInteract()
        {
            RoomController.Instance.GoTo(4);
            ScreenFlash.Flash(5, Ease.InQuad);
        }
    }
}