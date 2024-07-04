using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Painter3D
{
    public class EraseToggle : MonoBehaviour
    {
        [SerializeField]
        private Toggle eraseToggle;

        [SerializeField]
        private MousePainter mousePainter;

        private void Awake()
        {
            eraseToggle.onValueChanged.AddListener(delegate { EraseToggleChangeValue(); });
        }

        private void EraseToggleChangeValue()
        {
            mousePainter.Erase = eraseToggle.isOn;
        }

        private void OnDestroy()
        {
            eraseToggle.onValueChanged.RemoveAllListeners();
        }

    }
}