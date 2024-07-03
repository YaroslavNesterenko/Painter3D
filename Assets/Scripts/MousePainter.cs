using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Painter3D
{
    public class MousePainter : MonoBehaviour
    {

        [SerializeField]
        private Brush brush;

        [SerializeField]
        private bool erase = false;

        public bool Erase
        {
            get { return erase; }
            set { erase = value; }
        }

        [SerializeField]
        private BrushConfigurator brushConfigurator;

        private void Awake()
        {
            brushConfigurator.SetBrush(brush);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool success = true;
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.Log(hitInfo.collider.name);
                    var objectToPainting = hitInfo.transform.GetComponent<Painter>();
                    if (objectToPainting != null)
                    {
                        success = erase
                            ? objectToPainting.Erase(brush, hitInfo)
                            : objectToPainting.Paint(brush, hitInfo);

                    }

                    if (!success)
                    {
                        Debug.LogError("Painting Error!");
                    }
                }
            }

        }
    }
}