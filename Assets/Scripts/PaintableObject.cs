using Painter3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Painter3D
{
    public class PaintableObject : MonoBehaviour
    {
        private Renderer rend;
         
        private Texture2D texture;
        private Color[] cols;
        private Color[] colsDump;
        private bool isTextureCopied = false;
        private Vector2Int brushCenter;

        private bool isObjectWasPainted = false;
        public bool IsObjectWasPainted
        {
            get { return isObjectWasPainted; }
        }

        private void Start()
        {
            rend = GetComponent<Renderer>();
            PaintableObjectsContainer.AddPaintableObjectToList(this);
            CopyTextureOrCreateNewTexture();
        }

        public bool Paint(Brush brush, RaycastHit hitInfo)
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider is MeshCollider)
                {
                    return PaintByUV(brush, hitInfo.textureCoord);
                }
                return false;
            }
            return false;
        }


        private bool PaintByUV(Brush brush, Vector2 textureCoord)
        {
            isObjectWasPainted = true;

            brushCenter = PainterMathExtension.ConvertTextureCoordToXYOffset(textureCoord, texture);

            ChangePixelsColor(brush, texture, cols, brushCenter);

            texture.SetPixels(cols);

            // Copy the changes to the GPU. and don't recalculate mipmap levels.
            texture.Apply(false);

            return true;
        }

        
        private void CopyTextureOrCreateNewTexture()
        {
            if (!isTextureCopied)
            {
                if (rend.material.mainTexture != null)
                {
                    texture = Instantiate(rend.material.mainTexture) as Texture2D;                 
                    isTextureCopied = true;
                }
                else
                {
                    texture = new Texture2D(1024, 1024);
                }
                rend.material.mainTexture = texture;

                cols = texture.GetPixels();
                colsDump = texture.GetPixels();
            }
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

        public bool Erase(Brush brush, RaycastHit hitInfo)
        {
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider is MeshCollider)
                {
                    return EraseByUV(brush, hitInfo.textureCoord);
                }
                return false;
            }
            return false;
        }

        private bool EraseByUV(Brush brush, Vector2 textureCoord)
        {
            brushCenter = PainterMathExtension.ConvertTextureCoordToXYOffset(textureCoord, texture);

            ErasePixelColor(brush, texture, cols, colsDump, brushCenter);

            texture.SetPixels(cols);

            // Copy the changes to the GPU. and don't recalculate mipmap levels.
            texture.Apply(false);

            return true;
        }

        private void ErasePixelColor(Brush brush, Texture2D texture, Color[] cols, Color[] colorDump, Vector2Int brushCenter)
        {
            int texWidth = texture.width;

            Parallel.For(0, cols.Length,
                index =>
                {
                    Vector2Int point = PainterMathExtension.ConvertPixelNumberInPixelsArrayToCoordinate(index, texWidth);
                    bool result = PainterMathExtension.CheckIsPixelInsideBrushRadius(brushCenter, point, brush.Size);

                    if (result)
                    {
                        cols[index] = colsDump[index];
                    }
                }
            );
        }

        public void Clear()
        {
            if (isObjectWasPainted)
            {

                isObjectWasPainted = false;
                Array.Copy(colsDump, cols, cols.Length);
                texture.SetPixels(cols);

                texture.Apply(false);
            }
        }

        public Texture2D GetPaintedTexture()
        {
            return texture;
        }

        public void SetPaintedTexture(Texture2D loadedTexture)
        {
            texture = loadedTexture;
            rend.material.mainTexture = texture;

            cols = texture.GetPixels();
            colsDump = texture.GetPixels();
        }

        private void OnDestroy()
        {
            PaintableObjectsContainer.RemovePaintableObjectToList(this);
        }
    }
}