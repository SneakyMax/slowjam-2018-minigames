using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RoomController : MonoBehaviour
    {
        public static RoomController Instance { get; private set; }

        public int CurrentRoom;
        
        public GameObject Room1;
        public GameObject Room2;
        public GameObject Room3;
        public GameObject Room4;

        [Header("Edges")]
        public Transform Left;

        public Transform Right;
        public Transform Top;
        public Transform Bottom;

        public float Height => Top.position.y - Bottom.position.y;
        public float Width => Right.position.x - Left.position.x;

        private IList<GameObject> rooms;

        private void Awake()
        {
            Instance = this;
            rooms = new List<GameObject> { Room1, Room2, Room3, Room4 };
        }

        public void GoTo(int roomIndex)
        {
            if (roomIndex == CurrentRoom)
                return;

            rooms[CurrentRoom].SetActive(false);
            CurrentRoom = roomIndex;
            rooms[CurrentRoom].SetActive(true);
        }
    }
}