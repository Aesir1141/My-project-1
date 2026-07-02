// --- BẮT ĐẦU CODE THÊM MỚI ---
using System.Collections.Generic;
using UnityEngine;

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
            Instantiate(prefabToSpawn, targetSlot.spawnPoint.position, Quaternion.identity, targetSlot.spawnPoint);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ô trống trong Container hoặc thiếu Prefab!");
        }
    }
}
// --- KẾT THÚC CODE THÊM MỚI ---