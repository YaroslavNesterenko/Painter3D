using Painter3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Painter3D
{
    public class Painter : MonoBehaviour
    {


        public bool Paint(Brush brush, RaycastHit hitInfo)
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider is MeshCollider)
                {
                    return PaintByUV(brush, hitInfo.textureCoord);
                }

                return PaintByNearestTriangleSurface(brush, hitInfo.point);
            }

            return false;
        }


        private bool PaintByUV(Brush brush, Vector2 textureCoord)
        {
            throw new NotImplementedException();
        }
        private bool PaintByNearestTriangleSurface(Brush brush, Vector3 point)
        {
            throw new NotImplementedException();
        }

        public bool Erase(Brush brush, RaycastHit hitInfo)
        {
            throw new NotImplementedException();
        }
    }
}