using UnityEngine;

namespace Painter3D
{
    public class PainterMathExtension
    {

        public static Vector2Int ConvertTextureCoordToXYOffset(Vector2 textureCoord, Texture2D texture)
        {
            int xOffset = (int) (textureCoord.x * texture.width);
            int yOffset = (int) (textureCoord.y * texture.height);

            return new Vector2Int(xOffset, yOffset);
        }

        public static Vector2Int ConvertPixelNumberInPixelsArrayToCoordinate(int pixelNumber, int width)
        {
            Vector2Int coord = Vector2Int.zero;

            coord.x = pixelNumber % width;
            coord.y = pixelNumber / width;

            return coord;
        }

        public static bool CheckIsPixelInsideBrushRadius(Vector2Int center, Vector2Int point, int radius)
        {
            bool result = false;

            if (Mathf.Abs(center.x - point.x) < radius &&
                Mathf.Abs(center.y - point.y) < radius)
            {
                result = true;
            }
            //result = ((center.x - point.x) * (center.x - point.x) + (center.y - point.y) * (center.y - point.y)) <= radius * radius;

            return result;
        }
    }
}
