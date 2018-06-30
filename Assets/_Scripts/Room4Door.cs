using UnityEngine;

namespace Game
{
    public class Room4Door : MonoBehaviour
    {
        public static Room4Door Instance { get; private set; }

        private Animator animator;

        private void Awake()
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }

        public void Open()
        {
            animator.SetTrigger("Open");
        }
    }
}