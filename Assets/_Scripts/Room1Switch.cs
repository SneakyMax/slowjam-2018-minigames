using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Interactable))]
    public class Room1Switch : MonoBehaviour
    {
        public Room1Leaf[] Targets;

        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(Interact);
        }

        private void Interact()
        {
            foreach (var target in Targets)
            {
                target.Switch();
            }
        }
    }
}