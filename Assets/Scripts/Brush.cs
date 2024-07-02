using System;
using System.Collections.Generic;
using UnityEngine;

namespace Painter3D
{
    [Serializable]
    public class Brush : ICloneable
    {

        [SerializeField]
        private Color brushColor;
    
        [SerializeField, Range(0, 100)]
        private float brushScale = 10f;

        public Color BrushColor
        {
            get { return brushColor; }
            set { brushColor = value; }
        }

        public float Scale
        {
            get { return Mathf.Clamp01(brushScale); }
            set { brushScale = Mathf.Clamp01(value); }
        }

        public Brush(Color color, float scale)
        {
            BrushColor = color;
            Scale = scale;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
