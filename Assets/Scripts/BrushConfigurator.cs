using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Painter3D
{
    public class BrushConfigurator : MonoBehaviour
    {
        Brush currentBrush;

        [SerializeField]
        private Slider brushSizeSlider;

        [SerializeField] private Slider brushColorR;
        [SerializeField] private Slider brushColorG;
        [SerializeField] private Slider brushColorB;

        [SerializeField] private Image currentBrushColor;

        private void Awake()
        {
            brushSizeSlider.onValueChanged.AddListener(
                delegate
                {
                    ChangeBrushSize();
                }
            );

            brushColorR.onValueChanged.AddListener(delegate { ChangeBrushColorRGB(); });
            brushColorG.onValueChanged.AddListener(delegate { ChangeBrushColorRGB(); });
            brushColorB.onValueChanged.AddListener(delegate { ChangeBrushColorRGB(); });
        }

        private void Start()
        {
            ShowCurrentBrushColor();
        }

        public void SetBrush(Brush brush)
        {
            currentBrush = brush;
        }

        private void ChangeBrushSize()
        {
            currentBrush.Size = (int) brushSizeSlider.value;
        }

        private void ChangeBrushColorRGB()
        {
            Color newColor = currentBrush.BrushColor;
            newColor.r = brushColorR.value / 255f;
            newColor.g = brushColorG.value / 255f;
            newColor.b = brushColorB.value / 255f;
            currentBrush.BrushColor = newColor;

            ShowCurrentBrushColor();
        }

        private void ShowCurrentBrushColor()
        {
            currentBrushColor.color = currentBrush.BrushColor;
        }

    }

}