// Script: FinalCollider.cs
using UnityEngine;

public class FinalCollider : MonoBehaviour
{
    [Tooltip("Kéo object chứa script MarbleContainer vào đây")]
    public MarbleContainer container;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Marble marble = collision.GetComponent<Marble>();
        
        // --- BẮT ĐẦU CODE ĐƯỢC SỬA ---
        // Thêm điều kiện !marble.isProcessed để bỏ qua nếu bi đã được xử lý trước đó
        if (marble != null && !marble.isProcessed)
        {
            // Khoá viên bi lại ngay lập tức
            marble.isProcessed = true;
            // --- KẾT THÚC CODE ĐƯỢC SỬA ---

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