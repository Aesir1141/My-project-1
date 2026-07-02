// --- BẮT ĐẦU CODE ĐƯỢC SỬA ---
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ContainerSlot
{
    [Tooltip("Vị trí điểm spawn (Center) của ô này")]
    public Transform spawnPoint;
    
    [Tooltip("Mã màu đã claim ô này. Ban đầu để trống.")]
    public string claimedColorID = "";
}

public class MarbleContainer : MonoBehaviour
{
    [Tooltip("Danh sách 10 ô chứa bi")]
    public List<ContainerSlot> slots = new List<ContainerSlot>();

    [Tooltip("Tổng số bi cần thu thập để hoàn thành màn chơi")]
    public int totalMarblesInLevel;

    [Tooltip("Tên màn chơi (Scene) tiếp theo")]
    public string nextLevelName;

    private int currentCollectedMarbles = 0;

    // Hàm xử lý đưa bi vào hộp
    public void ProcessMarble(string colorID, GameObject prefabToSpawn)
    {
        ContainerSlot targetSlot = null;

        // Bươc 1: Kiểm tra xem màu này đã claim ô nào chưa
        foreach (var slot in slots)
        {
            if (slot.claimedColorID == colorID)
            {
                targetSlot = slot;
                break;
            }
        }

        // Bước 2: Nếu chưa có ô nào được claim bởi màu này, tìm ô trống đầu tiên
        if (targetSlot == null)
        {
            foreach (var slot in slots)
            {
                if (string.IsNullOrEmpty(slot.claimedColorID))
                {
                    slot.claimedColorID = colorID; // Claim ô này cho màu hiện tại
                    targetSlot = slot;
                    break;
                }
            }
        }

        // Bước 3: Spawn bi vào ô đã xác định
        if (targetSlot != null && prefabToSpawn != null)
        {
            // Đã sửa: Bỏ tham số targetSlot.spawnPoint để bi không kế thừa Scale của Spawner
            GameObject spawnedMarble = Instantiate(prefabToSpawn, targetSlot.spawnPoint.position, Quaternion.identity);
            
            // Xóa bỏ trạng thái nảy (bounce) bằng cách thay thế PhysicsMaterial2D
            Collider2D col = spawnedMarble.GetComponent<Collider2D>();
            if (col != null)
            {
                PhysicsMaterial2D noBounceMat = new PhysicsMaterial2D("NoBounce");
                noBounceMat.bounciness = 0f; // Triệt tiêu lực tưng nảy
                noBounceMat.friction = 0.4f; // Giữ lại ma sát cơ bản
                
                col.sharedMaterial = noBounceMat;
            }

            // Tăng số lượng bi đã thu thập
            currentCollectedMarbles++;

            // Kiểm tra điều kiện qua màn
            CheckWinCondition();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ô trống trong Container hoặc thiếu Prefab!");
        }
    }

    private void CheckWinCondition()
    {
        if (currentCollectedMarbles >= totalMarblesInLevel)
        {
            Debug.Log("Đã thu thập đủ bi! Đang tải màn chơi mới...");
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
        }
    }
}
// --- KẾT THÚC CODE ĐƯỢC SỬA ---