using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Room1LeafStack : MonoBehaviour
    {
        public Ladder Ladder;

        private IList<Room1Leaf> leaves;

        private void Awake()
        {
            leaves = GetComponentsInChildren<Room1Leaf>().OrderBy(x => x.transform.position.y).ToList();

            for (var i = 0; i < leaves.Count; i++)
            {
                leaves[i].Position = i;
                leaves[i].Changed += ResetLadderHeight;
            }
        }

        private void Start()
        {
            ResetLadderHeight();
        }

        private float GetMaxLeavesHeight()
        {
            var current = true;
            float maxHeight = transform.position.y;
            foreach (var leaf in leaves)
            {
                if (leaf.IsRight != current)
                {
                    current = !current;
                    maxHeight = leaf.transform.position.y;
                }
                else break;
            }

            return maxHeight;
        }

        private void ResetLadderHeight()
        {
            Ladder.MaxHeight = GetMaxLeavesHeight();
        }
    }
}