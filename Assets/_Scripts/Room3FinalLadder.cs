using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Room3FinalLadder : MonoBehaviour
    {
        public static Room3FinalLadder Instance { get; private set; }

        public float MoveTime = 3;

        public float MoveDistance = 1;

        private bool moved;

        private void Awake()
        {
            Instance = this;
        }

        public void Move()
        {
            if (moved)
                return;
            transform.DOLocalMoveY(-MoveDistance, MoveTime).SetRelative();
            moved = true;
        }
    }
}