using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Room1LeafStack : MonoBehaviour
    {
        public event Action Completed;

        public Ladder Ladder;

        private IList<Room1Leaf> leaves;
        private float originalLadderHeight;

        public bool IsComplete { get; set; }

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
            originalLadderHeight = Ladder.MaxHeight;
            ResetLadderHeight();
        }

        private float GetMaxLeavesHeight()
        {
            var current = true;
            float maxHeight = transform.position.y;
            var last = leaves.Last();
            foreach (var leaf in leaves)
            {
                if (leaf.IsRight != current)
                {
                    current = !current;
                    maxHeight = leaf.transform.position.y;

                    if (leaf == last)
                    {
                        maxHeight = originalLadderHeight;
                        IsComplete = true;
                        Completed?.Invoke();
                    }
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