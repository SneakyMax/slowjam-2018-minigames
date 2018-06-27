using UnityEngine;

namespace Game
{
    public class RoomTransition : MonoBehaviour
    {
        public int TargetRoomIndex;

        public bool IsDown;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) 
                return;

            if (PlayerController.Instance.CannotTransition)
            {
                PlayerController.Instance.CannotTransition = false;
                return;
            }
            
            RoomController.Instance.GoTo(TargetRoomIndex);

            var playerTransform = PlayerController.Instance.gameObject.transform;

            playerTransform.position = IsDown 
                ? new Vector3(playerTransform.position.x, playerTransform.position.y + RoomController.Instance.Height, playerTransform.position.z) 
                : new Vector3(playerTransform.position.x, playerTransform.position.y - RoomController.Instance.Height, playerTransform.position.z);

            PlayerController.Instance.ImmediatelyGrabLadder();
            PlayerController.Instance.CannotTransition = true;

        }
    }
}
