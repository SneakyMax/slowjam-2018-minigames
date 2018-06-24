using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game
{
    [ExecuteInEditMode]
    public class DesaturatorRuntime : MonoBehaviour
    {
        public static DesaturatorRuntime Instance { get; private set; }

        public ColorNode[] Nodes;

        public float[] SaturationArray;

        private PostProcessVolume volume;
        private PostProcessProfile profile;
        private Desaturator settings;

        public void Awake()
        {
            Instance = this;
            SaturationArray = Enumerable.Range(0, 360).Select(x => 1.0f).ToArray();
        }

        public void Start()
        {
            volume = GetComponent<PostProcessVolume>();
            profile = volume.profile;
            profile.TryGetSettings(out settings);
        }

        private float[] GetSaturationArray()
        {
            var nodes = new LinkedList<ColorNode>(Nodes.OrderBy(x => x.Hue).ToList());
            if (nodes.Count == 0)
                nodes.AddFirst(new ColorNode { Hue = 0, Value = 0 });
            var current = nodes.First;

            var arr = new float[360];
            for (var i = 0; i < 360; i++)
            {
                var next = current.Next ?? current.List.First;

                // If we're past the hue of the next node, use the next node as the current node
                if (i > next.Value.Hue && next != nodes.First)
                {
                    current = current.Next ?? current.List.First;
                    next = current.Next ?? current.List.First;
                }

                var a = (i - current.Value.Hue) / (next.Value.Hue - current.Value.Hue);
                if (next == nodes.First)
                    a = (i - current.Value.Hue) / (next.Value.Hue + 360 - current.Value.Hue);

                var a2 = (1 - Mathf.Cos(a * Mathf.PI)) / 2;
                arr[i] = current.Value.Value * (1 - a2) + next.Value.Value * a2;
                //arr[i] = Mathf.Lerp(current.Value.Value, next.Value.Value, a);
            }

            return arr;
        }

        public void Update()
        {
            SaturationArray = GetSaturationArray();

            if (Instance == null)
                Instance = this;
        }
    }

    [Serializable]
    public struct ColorNode
    {
        public string ReferenceName;

        [Range(0, 360)]
        public float Hue;

        [Range(0, 1)]
        public float Value;
    }
}

