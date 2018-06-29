using UnityEngine;

namespace Game
{
    public class Room3PuzzleButton : MonoBehaviour
    {
        public Room3PuzzlePlacer Placer;

        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
        }

        private void OnInteract()
        {
            if (Room3Controller.Instance.Solved)
                return;

            Placer.Rotate();
        }
    }
}