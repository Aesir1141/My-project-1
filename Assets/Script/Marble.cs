// --- BẮT ĐẦU CODE THÊM MỚI ---
using UnityEngine;

public class Marble : MonoBehaviour
{
    [Tooltip("Mã màu hoặc tên màu để phân biệt các viên bi (VD: 'Blue', 'Red')")]
    public string colorID;

    [Tooltip("Prefab của viên bi sẽ được spawn trong hộp (Có thể để trống nếu muốn dùng chính prefab này)")]
    public GameObject prefabToSpawnInContainer;
}
// --- KẾT THÚC CODE THÊM MỚI ---