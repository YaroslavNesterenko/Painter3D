using System;
using System.Collections.Generic;
using UnityEngine;

namespace Painter3D
{
    [Serializable]
    public class Brush : ICloneable
    {
        public enum ColorBlendType
        {
            UseColor,
            UseTexture
        }

        [SerializeField]
        private Color brushColor;

        [SerializeField]
        private Texture brushTexture;

        [SerializeField, Range(0, 1)]
        private float brushScale = 0.1f;

        [SerializeField, Range(0, 360)]
        private float brushRotateAngle = 0;

        [SerializeField]
        private ColorBlendType colorBlendType = ColorBlendType.UseColor;

        public Color BrushColor
        {
            get { return brushColor; }
            set { brushColor = value; }
        }

        public Texture BrushTexture
        {
            get { return brushTexture; }
            set { brushTexture = value; }
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

        public ColorBlendType ColorBlendingType
        {
            get { return colorBlendType; }
            set { colorBlendType = value; }
        }

        public Brush(Color color, Texture texture, float scale)
        {
            BrushColor = color;
            BrushTexture = texture;
            Scale = scale;
        }

        public Brush(Color color, Texture texture, float scale, ColorBlendType colorBlendingType)
            : this(color, texture, scale)
        {
            ColorBlendingType = colorBlendingType;
        }





















        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
