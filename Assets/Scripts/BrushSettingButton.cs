using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSettingButton : MonoBehaviour
{
    [SerializeField]
    private Button settingButton;

    [SerializeField]
    private GameObject settingPanel;

    private void Awake()
    {
        settingButton.onClick.AddListener(SettingButtonClick);
    }

    private void SettingButtonClick()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    private void OnDestroy()
    {
        settingButton.onClick.RemoveListener(SettingButtonClick);
    }
}
