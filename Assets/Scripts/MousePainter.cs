using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool success = true;
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
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