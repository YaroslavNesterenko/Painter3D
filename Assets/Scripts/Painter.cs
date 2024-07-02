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
            Renderer rend = GetComponent<Renderer>();

            // Duplicate the original texture and assign to the material
            Texture2D texture = Instantiate(rend.material.mainTexture) as Texture2D;
            rend.material.mainTexture = texture;
        
            //int mipCount = Mathf.Min(3, texture.mipmapCount);

            Color[] cols = texture.GetPixels();

            int xOffset = (int) (textureCoord.x * texture.width);
            int yOffset = (int) (textureCoord.y * texture.height);

            cols[ yOffset * texture.width + xOffset] = Color.red;

            texture.SetPixels(cols);

            // Copy the changes to the GPU. and don't recalculate mipmap levels.
            texture.Apply(false);

            return true;
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