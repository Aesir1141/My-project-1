using UnityEngine;

// Bắt đầu code được sửa hoặc thêm
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Stage_2_Obs : MonoBehaviour
{
    public int xSegments = 50;           // Tăng nhẹ số lưới để đỉnh sóng bo tròn mượt nhất
    public float amplitude = 1.2f;       
    public float speed = 3.0f;           
    public float waveFrequency = 4.0f;   
    
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices; 

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateSubdividedQuad();
    }

    void GenerateSubdividedQuad()
    {
        mesh = new Mesh();
        mesh.name = "WavyQuad";

        int numVertices = (xSegments + 1) * 2; 
        baseVertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        
        float halfWidth = 0.5f;
        float halfHeight = 0.5f;
        
        for (int i = 0; i <= xSegments; i++)
        {
            float t = (float)i / xSegments;
            float xPos = Mathf.Lerp(-halfWidth, halfWidth, t);
            
            baseVertices[i] = new Vector3(xPos, -halfHeight, 0);
            uvs[i] = new Vector2(t, 0);
            
            baseVertices[i + xSegments + 1] = new Vector3(xPos, halfHeight, 0);
            uvs[i + xSegments + 1] = new Vector2(t, 1);
        }

        int numTriangles = xSegments * 6;
        int[] triangles = new int[numTriangles];
        int ti = 0;
        
        for (int i = 0; i < xSegments; i++)
        {
            int bottomLeft = i;
            int bottomRight = i + 1;
            int topLeft = i + xSegments + 1;
            int topRight = i + xSegments + 2;

            triangles[ti++] = bottomLeft;
            triangles[ti++] = topLeft;
            triangles[ti++] = bottomRight;

            triangles[ti++] = bottomRight;
            triangles[ti++] = topLeft;
            triangles[ti++] = topRight;
        }

        mesh.vertices = baseVertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    void Update()
    {
        if (mesh == null) return;

        Vector3[] vertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 v = baseVertices[i];
            
            if (v.y > 0) 
            {
                float normalizedDist = Mathf.Clamp01(Mathf.Abs(v.x) / 0.5f);
                float centerFocusMultiplier = Mathf.SmoothStep(1f, 0f, normalizedDist);
                
                // Thay thế Sóng Di Chuyển (Traveling Wave) bằng Sóng Đứng (Standing Wave)
                // Tách riêng tính toán vị trí (v.x) và thời gian (Time.time)
                float wave = Mathf.Cos(v.x * waveFrequency) * Mathf.Sin(Time.time * speed) * (amplitude * centerFocusMultiplier); 
                
                v.y = baseVertices[i].y + wave;
            }
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
// Kết thúc code được sửa hoặc thêm