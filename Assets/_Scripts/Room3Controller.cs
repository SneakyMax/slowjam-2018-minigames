using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class Room3Controller : MonoBehaviour
    {
        public static Room3Controller Instance { get; private set; }

        public Room3PuzzlePlacer[] Placers;

        public WanderingCharacter[] Characters;

        public bool Solved { get; private set; }

        public float FlashTime;

        public event Action OnSolved;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var placer in Placers)
            {
                placer.Changed += Check;
            }
        }

        private void Check()
        {
            if (Solved)
                return;

            var isValid = true;
            foreach (var placer in Placers)
            {
                isValid = isValid && placer.Check();
            }

            if (isValid)
            {
                StartCoroutine(DoSolved());
            }
        }

        private IEnumerator DoSolved()
        {
            Solved = true;
            OnSolved?.Invoke();

            yield return new WaitForSeconds(0.75f);

            CameraController.Instance.ShakeOn();
            Room3FinalLadder.Instance.Move();
            yield return new WaitForSeconds(Room3FinalLadder.Instance.MoveTime);
            CameraController.Instance.ShakeOff();

            foreach (var character in Characters)
            {
                character.Sleep();
            }

            ScreenFlash.Flash(FlashTime);
            GameController.Instance.MoreColor();
        }
    }
}