using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Room2Controller : MonoBehaviour
    {
        public static Room2Controller Instance { get; private set; }

        private enum State
        {
            ShowingPattern,
            WaitingForNextPlayerInput,
            FlashingAll,
            Inactive
        };

        public Room2LightCharacter[] Characters;

        public float TimePerFlash;
        public float TimeBetweenFlashes;

        public int[] PatternCounts;

        private IList<IList<Color>> patterns;
        private IDictionary<Color, Room2LightCharacter> colorMap;

        private State currentState;
        private int currentPattern = 0;
        public float FlashTime;

        private int currentPositionInPattern;

        private Coroutine currentCoroutine;

        private void Awake()
        {
            Instance = this;

            var availableColors = new[] { Color.red, Color.blue, Color.magenta, Color.cyan, Color.yellow, Color.green };
            availableColors = availableColors.Shuffle().ToArray();
            colorMap = new Dictionary<Color, Room2LightCharacter>();

            for (var i = 0; i < Characters.Length; i++)
            {
                Characters[i].Color = availableColors[i];
                colorMap[availableColors[i]] = Characters[i];
            }

            var allColors = Characters.Select(x => x.Color).ToList();

            patterns = new List<IList<Color>>();
            for (var i = 0; i < 3; i++)
                patterns.Add(Enumerable.Range(0, PatternCounts[i]).Select(x => Random.Range(0, allColors.Count)).Select(x => allColors[x]).ToList());

            currentState = State.ShowingPattern;

            currentCoroutine = StartCoroutine(ShowPattern());
        }

        private void DoShowPattern()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentState = State.ShowingPattern;
            currentCoroutine = StartCoroutine(ShowPattern());
            currentPositionInPattern = 0;
        }

        private void DoFlashAll()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentState = State.FlashingAll;
            currentCoroutine = StartCoroutine(FlashAll());
            currentPositionInPattern = 0;
        }

        private IEnumerator ShowPattern()
        {
            while (true)
            {
                if (currentState != State.ShowingPattern)
                    break;

                for (var i = 0; i < PatternCounts[currentPattern]; i++)
                {
                    var character = colorMap[patterns[currentPattern][i]];
                    character.LightOn();
                    yield return new WaitForSeconds(TimePerFlash);
                    character.LightOff();
                    yield return new WaitForSeconds(TimeBetweenFlashes);
                }

                foreach (var character in Characters)
                {
                    character.LightOn();
                }
                yield return new WaitForSeconds(TimePerFlash);
                foreach (var character in Characters)
                {
                    character.LightOff();
                }
                yield return new WaitForSeconds(TimeBetweenFlashes);
            }
        }

        private IEnumerator FlashAll()
        {
            for (var i = 0; i < 5; i++)
            {
                foreach (var character in Characters)
                {
                    character.LightOn();
                }
                yield return new WaitForSeconds(0.2f);
                foreach (var character in Characters)
                {
                    character.LightOff();
                }
                yield return new WaitForSeconds(0.2f);
            }

            DoShowPattern();
        }

        public void Mark(Color color)
        {
            if (currentState == State.FlashingAll || currentState == State.Inactive)
                return;

            if (currentState == State.ShowingPattern)
            {
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);
                currentState = State.WaitingForNextPlayerInput;
                foreach (var character in Characters)
                {
                    character.LightOff();
                }
            }

            var neededColor = patterns[currentPattern][currentPositionInPattern];
            if (color == neededColor)
            {
                currentPositionInPattern++;
                colorMap[neededColor].FlashOnce();
            }
            else
            {
                DoFlashAll();
                return;
            }

            if (currentPositionInPattern == patterns[currentPattern].Count)
            {
                StartCoroutine(CompletedPattern());
            }

            if (currentPattern > patterns.Count - 1)
            {
                StartCoroutine(CompletedPuzzle());
                // Completed puzzle
            }
        }

        private IEnumerator CompletedPattern()
        {
            currentPattern++;
            currentPositionInPattern = 0;

            foreach (var obj in GameObject.FindGameObjectsWithTag("ConveyorBoxes"))
            {
                var boxes = obj.GetComponent<Room2ConveyorBoxes>();
                if (boxes.Puzzle == currentPattern - 1)
                {
                    boxes.Move();
                }
            }

            CameraController.Instance.ShakeOn();
            foreach (var character in Characters)
            {
                character.LightOn();
            }
            yield return new WaitForSeconds(5);
            CameraController.Instance.ShakeOff();
            foreach (var character in Characters)
            {
                character.LightOff();
            }
            yield return new WaitForSeconds(1);

            if (currentPattern < patterns.Count)
                DoShowPattern();
        }

        private IEnumerator CompletedPuzzle()
        {
            currentState = State.Inactive;

            foreach (var character in Characters)
            {
                character.GetComponent<WanderingCharacter>().Sleep();
            }

            yield return new WaitForSeconds(0.75f);
            ScreenFlash.Flash(FlashTime);

        }
    }
}