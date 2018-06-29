using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Room2ConveyorBoxes : MonoBehaviour
    {
        public float MoveTime = 5;

        public float MoveDistance = 1;

        public int Puzzle = 0;

        public void Move()
        {
            transform.DOLocalMoveX(transform.position.x - MoveDistance, MoveTime);
        }
    }
}