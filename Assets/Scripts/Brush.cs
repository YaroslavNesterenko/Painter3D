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
        private int brushSize = 10;

        public Color BrushColor
        {
            get { return brushColor; }
            set { brushColor = value; }
        }

        public int Size
        {
            get { return brushSize; }
            set { brushSize = (int) Mathf.Clamp((float) value, 0f, 100f); }
        }

        public Brush(Color color, int size)
        {
            BrushColor = color;
            Size = size;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
