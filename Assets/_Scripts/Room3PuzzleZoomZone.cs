using UnityEngine;

namespace Game
{
    public class Room3PuzzleZoomZone : MonoBehaviour
    {
        private void Start()
        {
            Room3Controller.Instance.OnSolved += OnSolved;
        }

        private void OnSolved()
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}