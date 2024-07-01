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
    
        [SerializeField, Range(0, 1)]
        private float brushScale = 0.1f;

        [SerializeField, Range(0, 360)]
        private float brushRotateAngle = 0;

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

        public float BrushRotateAngle
        {
            get { return brushRotateAngle; }
            set { brushRotateAngle = value; }
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
