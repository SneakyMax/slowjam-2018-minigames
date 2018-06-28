using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Interactable))]
    public class Room1Switch : MonoBehaviour
    {
        public Room1Leaf[] Targets;

        public SpriteRenderer SwitchSprite;

        public Sprite Off;
        public Sprite On;

        private bool isOn;

        private void Start()
        {
            isOn = false;
            GetComponent<Interactable>().OnInteract.AddListener(Interact);
        }

        private void Interact()
        {
            if (Room1Controller.Instance.Finished)
                return;

            foreach (var target in Targets)
            {
                target.Switch();
            }

            isOn = !isOn;

            SwitchSprite.sprite = isOn ? On : Off;
        }
    }
}