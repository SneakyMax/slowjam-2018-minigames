using UnityEngine;

namespace Game
{
    public class Room4ComputerDialogArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Room4Controller.Instance.ShowComputerDialog();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Room4Controller.Instance.HideComputerDialog();
            }
        }
    }
}