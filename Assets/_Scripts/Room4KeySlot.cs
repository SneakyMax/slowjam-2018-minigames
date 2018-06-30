using UnityEngine;

namespace Game
{
    public class Room4KeySlot : MonoBehaviour
    {
        [Range(0, 2)]
        public int NeededKeyIndex;

        public SpriteRenderer ActiveSprite;

        private void Start()
        {
            GetComponent<Interactable>().OnInteract.AddListener(OnInteract);
            ActiveSprite.enabled = false;
        }

        private void OnInteract()
        {
//            var holding = PlayerController.Instance.Holding;
//            
//            if (holding == null)
//                return;
//            
//            var key = holding.GetComponent<Room4InventoryKey>();
//            if (key == null || key.Index != NeededKeyIndex)
//                return;
//            
//            var item = PlayerController.Instance.TakeItem();
            Room4Controller.Instance.OnInsertedKey();
            GetComponent<Interactable>().Can = false;
//
//            Destroy(item);
//
//            ActiveSprite.enabled = true;
        }
    }
}