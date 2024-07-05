using Painter3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Painter3D
{
    public class PaintableObject : MonoBehaviour
    {
        private Renderer rend;
         
        private Texture2D texture;
        private int texWidth;
        private Color[] cols;
        private Color[] colsDump;
        private bool isTextureCopied = false;
        private Vector2Int brushCenter = Vector2Int.zero;
        private Vector2Int previousBrushCenter = Vector2Int.zero;

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

            Profiler.BeginSample("PaintByUV");
            if (previousBrushCenter == Vector2Int.zero)
            {
                ChangePixelsColor(brush, texture, cols, brushCenter);
            }
            else
            {
                FindPointsOnLine(brush, texture, cols, brushCenter, previousBrushCenter);
            }
            Profiler.EndSample();
            previousBrushCenter = brushCenter;

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

                texWidth = texture.width;
                cols = texture.GetPixels();
                colsDump = texture.GetPixels();
            }
        }

        private void ChangePixelsColor(Brush brush, Texture2D texture, Color[] cols, Vector2Int brushCenter)
        {
            //Profiler.BeginSample("ChangePixelsColor");
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

        private void FindPointsOnLine(Brush brush, Texture2D texture, Color[] cols, Vector2Int brushCenter, Vector2Int previousBrushCenter)
        {
            var diff_X = brushCenter.x - previousBrushCenter.x;
            var diff_Y = brushCenter.y - previousBrushCenter.y;
            int pointNum = 3;

            var interval_X = diff_X / (pointNum + 1);
            var interval_Y = diff_Y / (pointNum + 1);

            List<Vector2Int> pointList = new List<Vector2Int>();
            for (int i = 1; i <= pointNum; i++)
            {
                pointList.Add(new Vector2Int(previousBrushCenter.x + interval_X * i, previousBrushCenter.y + interval_Y * i));
            }
            
            foreach (var item in pointList)
            {

                ChangePixelsColor(brush, texture, cols, item);
            }
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
            //if (isObjectWasPainted)
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
            //colsDump = texture.GetPixels();
        }

        private void OnDestroy()
        {
            PaintableObjectsContainer.RemovePaintableObjectToList(this);
        }
    }
}