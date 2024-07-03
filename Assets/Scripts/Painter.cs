using Painter3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                texture = new Texture2D(1024, 1024);
            }
            rend.material.mainTexture = texture;

            Color[] cols = texture.GetPixels();

            Vector2Int brushCenter = PainterMathExtension.ConvertTextureCoordToXYOffset(textureCoord, texture);

            ChangePixelsColor(brush, texture, cols, brushCenter);

            texture.SetPixels(cols);

            // Copy the changes to the GPU. and don't recalculate mipmap levels.
            texture.Apply(false);

            return true;
        }

        private void ChangePixelsColor(Brush brush, Texture2D texture, Color[] cols, Vector2Int brushCenter)
        {
            int texWidth = texture.width;
            Parallel.For(0, cols.Length,
                index =>
                {
                    Vector2Int point = PainterMathExtension.ConvertPixelNumberInPixelsArrayToCoordinate(index, texWidth);
                    bool result = PainterMathExtension.CheckIsPixelInsideBrushRadius(brushCenter, point, brush.Size);

                    if (result)
                    {
                        cols[index] = brush.BrushColor;
                    }
                }
            );
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