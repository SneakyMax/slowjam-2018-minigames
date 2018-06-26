using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class RoomTransition : MonoBehaviour
    {
        public Scene Target;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneController.Instance.GoTo(new Scene());
            }
        }
    }
}
