using System.Collections;
using UnityEngine;

namespace Game
{
    public class Room1Controller : MonoBehaviour
    {
        public static Room1Controller Instance { get; private set; }

        public Room1LeafStack TargetStack;

        public float FlashTime;

        private void Awake()
        {
            Instance = this;

            TargetStack.Completed += () => StartCoroutine(Room1Finished());
        }

        private IEnumerator Room1Finished()
        {
            Finished = true;
            yield return new WaitForSeconds(0.75f);
            ScreenFlash.Flash(FlashTime);
        }

        public bool Finished { get; private set; }
    }
}