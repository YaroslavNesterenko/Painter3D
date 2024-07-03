using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Painter3D
{
    public class MousePainter : MonoBehaviour
    {

        EventSystem eventSystem;

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
            eventSystem = EventSystem.current;
        }


        private void Update()
        {
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
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

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}