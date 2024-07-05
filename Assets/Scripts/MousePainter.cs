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

        
        Ray ray;
        bool success;
        RaycastHit hitInfo;
        PaintableObject objectToPainting;

        private void Awake()
        {
            brushConfigurator.SetBrush(brush);
            eventSystem = EventSystem.current;
        }


        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                success = true;
                
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //Debug.Log(hitInfo.collider.name);
                    objectToPainting = hitInfo.transform.GetComponent<PaintableObject>();
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

        PointerEventData eventDataCurrentPosition;
        List<RaycastResult> results;

        private bool IsPointerOverUIObject()
        {
            eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }      
    }
}