// --- BẮT ĐẦU CODE THÊM MỚI ---
using UnityEngine;

public class FinalCollider : MonoBehaviour
{
    [Tooltip("Kéo object chứa script MarbleContainer vào đây")]
    public MarbleContainer container;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem object chạm vào có phải là bi không
        Marble marble = collision.GetComponent<Marble>();
        
        if (marble != null)
        {
            // Xác định prefab cần spawn (nếu viên bi không set prefab riêng thì dùng chính nó)
            GameObject spawnPrefab = marble.prefabToSpawnInContainer != null ? marble.prefabToSpawnInContainer : marble.gameObject;

            // Gọi Container để xử lý logic claim màu và spawn
            if (container != null)
            {
                container.ProcessMarble(marble.colorID, spawnPrefab);
            }

            // Xoá viên bi vừa chạm vào vạch đích khỏi môi trường simulation
            Destroy(marble.gameObject);
        }
    }
}
// --- KẾT THÚC CODE THÊM MỚI ---