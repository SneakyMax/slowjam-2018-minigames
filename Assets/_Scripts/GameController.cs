using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        private ColorNodeList[] colorLevels;

        public int CurrentColorIndex;

        private void Awake()
        {
            Instance = this;
            colorLevels = GetComponents<ColorNodeList>();
        }

        private void Start()
        {
            UpdateColor();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void MoreColor()
        {
            CurrentColorIndex++;
            UpdateColor();
        }

        private void UpdateColor()
        {
            var colorLevel = colorLevels.OrderBy(x => x.Index).ElementAt(CurrentColorIndex);
            DesaturatorRuntime.Instance.Nodes = colorLevel.Nodes;
        }
    }
}