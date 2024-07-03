using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueShower : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMPro.TMP_Text valueText;

    void Awake()
    {
        slider.onValueChanged.AddListener(delegate { SladerValueChanged(); });
        SladerValueChanged();
    }

    private void  SladerValueChanged()
    {
        valueText.text = slider.value.ToString();
    }
}
