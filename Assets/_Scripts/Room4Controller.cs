using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game
{
    public class Room4Controller : MonoBehaviour
    {
        public static Room4Controller Instance { get; private set; }

        public RectTransform DialogBox;

        public TextMeshProUGUI[] RiddlesPrefabs;

        public Room4ExitDoorArea ExitDoorArea;

        public HiddenKey[] HiddenKeys;

        public float DoorOpenTime;

        private GameObject currentDialog;

        private int currentRiddle = -1;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowComputerDialog()
        {
            if (currentDialog != null)
                HideComputerDialog();

            // Riddles only start when you see the computer for the first time.
            if (currentRiddle == -1)
            {
                NextRiddle();
            }

            var prefab = RiddlesPrefabs[currentRiddle].gameObject;
            currentDialog = Instantiate(prefab, DialogBox, false);

            DialogBox.gameObject.SetActive(true);
        }

        public void HideComputerDialog()
        {
            if (currentDialog != null)
                Destroy(currentDialog);

            DialogBox.gameObject.SetActive(false);
        }

        public void OnInsertedKey()
        {
            ScreenFlash.Flash(1.0f);
            NextRiddle();
        }

        private void NextRiddle()
        {
            currentRiddle++;

            if (currentRiddle == RiddlesPrefabs.Length)
            {
                StartCoroutine(CompletedPuzzle());
            }
            else
            {
                HiddenKeys.OrderBy(x => x.Index).ElementAt(currentRiddle).Activate();
            }
        }

        private IEnumerator CompletedPuzzle()
        {
            CameraController.Instance.ShakeOn();
            Room4Door.Instance.Open();
            yield return new WaitForSeconds(DoorOpenTime);
            CameraController.Instance.ShakeOff();
            ExitDoorArea.GetComponent<Interactable>().Can = true;
        }
    }
}