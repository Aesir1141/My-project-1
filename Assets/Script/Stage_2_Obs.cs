using UnityEngine;

// Bắt đầu code được sửa hoặc thêm
public class Stage_2_Obs : MonoBehaviour
{
    public float amplitude = 0.5f; // Độ sâu lún xuống
    public float speed = 2.0f;     // Tốc độ lặp lại
    
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices; 

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            // Lấy mesh từ object
            mesh = meshFilter.mesh;
            baseVertices = mesh.vertices;
        }
    }

    void Update()
    {
        if (mesh == null) return;

        Vector3[] vertices = new Vector3[baseVertices.Length];
        // Tính giá trị lún dựa trên hàm Sin (để tạo vòng lặp lên/xuống liên tục)
        float offset = Mathf.Sin(Time.time * speed) * amplitude;

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 v = baseVertices[i];
            
            // Chỉ tác động vào phần cạnh trên (dựa vào tọa độ Y)
            // Kiểm tra các đỉnh nằm phía trên (Ví dụ: v.y > 0.4f)
            if (v.y > 0.4f) 
            {
                // Công thức Parabol: f(x) = a * x^2
                // dist là khoảng cách từ đỉnh hiện tại đến tâm X của object
                float dist = v.x; 
                
                // Độ biến dạng giảm dần từ tâm ra hai bên để tạo hình Parabol
                // Khi dist = 0 (tâm), lún sâu nhất = offset
                // Khi dist càng xa tâm, lún càng ít
                float deformation = offset * (1 - (dist * dist * 4.0f)); 
                
                v.y = baseVertices[i].y + deformation;
            }
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        
        // Cập nhật Collider nếu bạn có dùng MeshCollider
        if (GetComponent<MeshCollider>() != null)
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
// Kết thúc code được sửa hoặc thêm