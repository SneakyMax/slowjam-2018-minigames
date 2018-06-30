using UnityEngine;

namespace Game
{
    public class HiddenKey : MonoBehaviour
    {
        public Room4InventoryKey KeyPrefab;

        public int Index => KeyPrefab.Index;

        private void Start()
        {
            GetComponent<Interactable>().Can = false;
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
        }

        private void OnInteract()
        {
            var key = Instantiate(KeyPrefab.gameObject);
            PlayerController.Instance.GiveItem(key);
            GetComponent<Interactable>().Can = false;
        }

        public void Activate()
        {
            GetComponent<Interactable>().Can = true;
        }
    }
}