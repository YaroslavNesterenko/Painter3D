using UnityEngine;

public class BrushPainter : MonoBehaviour
{
    public Shader brushShader;
    public Texture brushTexture;
    public Color brushColor = Color.white;
    public float brushSize = 0.1f;
    public LayerMask paintableLayer;

    private RaycastHit hit;
    private Renderer lastHitRenderer;
    private Material brushMaterial;

    void Start()
    {
        Application.targetFrameRate = 60;

        if (brushShader == null)
        {
            Debug.LogError("Brush shader is not assigned!");
            enabled = false;
            return;
        }

        brushMaterial = new Material(brushShader);
        brushMaterial.SetTexture("_MainTex", brushTexture);
        brushMaterial.SetColor("_Color", brushColor);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, paintableLayer))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();

                if (renderer != null /*&& renderer != lastHitRenderer*/)
                {
                    Paint(renderer, hit.textureCoord);
                    lastHitRenderer = renderer;
                }
            }
        }
        else
        {
            lastHitRenderer = null;
        }
    }

    void Paint(Renderer renderer, Vector2 uv)
    {
        Texture2D tex = (Texture2D) renderer.material.mainTexture;
        Vector2 pixelUV = uv * tex.width;
        int x = (int) pixelUV.x;
        int y = (int) pixelUV.y;

        DrawBrush(tex, x, y);
        tex.Apply();
    }

    void DrawBrush(Texture2D tex, int centerX, int centerY)
    {
        int brushWidth = Mathf.RoundToInt(tex.width * brushSize);
        int brushHeight = Mathf.RoundToInt(tex.height * brushSize);
        int startX = centerX - brushWidth / 2;
        int startY = centerY - brushHeight / 2;

        for (int x = startX; x < startX + brushWidth; x++)
        {
            for (int y = startY; y < startY + brushHeight; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY)) <= brushWidth / 2)
                {
                    tex.SetPixel(x, y, brushColor);
                }
            }
        }
    }

    void OnDestroy()
    {
        Destroy(brushMaterial);
    }
}
