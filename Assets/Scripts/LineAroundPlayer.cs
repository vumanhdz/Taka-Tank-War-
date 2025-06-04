using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineAroundPlayer : MonoBehaviour
{
    public Transform player;
    public int segments = 100;
    PlayerCtrl playerCtrl;

    private LineRenderer lineRenderer;

    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (player != null)
        {
            DrawCircle();
        }
    }

    void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * playerCtrl.detectRadius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * playerCtrl.detectRadius;
            Vector3 pos = new Vector3(x, 0f, z) + player.position;
            lineRenderer.SetPosition(i, pos);
            angle += 360f / segments;
        }
    }
}