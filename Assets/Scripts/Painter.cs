using Painter3D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Painter3D
{
    public class Painter : MonoBehaviour
    {
        Renderer rend;


        private void Start()
        {
            rend = GetComponent<Renderer>();
        }

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
            
            Texture2D texture;
            if (rend.material.mainTexture != null)
            {
                // Duplicate the original texture and assign to the material
                texture = Instantiate(rend.material.mainTexture) as Texture2D;
            }
            else
            {
                texture = new Texture2D(64, 64);
            }
            rend.material.mainTexture = texture;

            Color[] cols = texture.GetPixels();

            Vector2Int brushCenter = PainterMathExtension.ConvertTextureCoordToXYOffset(textureCoord, texture);

            for (int i = 0; i < cols.Length; i++)
            {
                //Debug.Log(PainterMathExtension.ConvertPixelNumberInPixelsArrayToCoordinate(i, texture.width));
                Vector2Int point = PainterMathExtension.ConvertPixelNumberInPixelsArrayToCoordinate(i, texture.width);
                bool result = PainterMathExtension.CheckIsPixelInsideBrushRadius(brushCenter, point, brush.Size);

                if (result)
                {
                    cols[i] = brush.BrushColor;
                }
            }


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