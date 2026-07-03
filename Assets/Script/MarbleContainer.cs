using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Đã thêm để sử dụng UnityEvent

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

    // --- BẮT ĐẦU CODE ĐƯỢC THÊM/SỬA ---
    [Tooltip("Event được gọi sau khi camera đã di chuyển xong. Gắn hàm spawn của Spawner mới vào đây.")]
    public UnityEvent onCameraMovedToNewLevel;

    private List<GameObject> spawnedMarblesList = new List<GameObject>(); // Dùng để lưu trữ và xóa bi
    // --- KẾT THÚC CODE ĐƯỢC THÊM/SỬA ---

    private int currentCollectedMarbles = 0;

    // Hàm xử lý đưa bi vào hộp
    public void ProcessMarble(string colorID, GameObject prefabToSpawn)
    {
        ContainerSlot targetSlot = null;

        foreach (var slot in slots)
        {
            if (slot.claimedColorID == colorID)
            {
                targetSlot = slot;
                break;
            }
        }

        if (targetSlot == null)
        {
            foreach (var slot in slots)
            {
                if (string.IsNullOrEmpty(slot.claimedColorID))
                {
                    slot.claimedColorID = colorID;
                    targetSlot = slot;
                    break;
                }
            }
        }

        if (targetSlot != null && prefabToSpawn != null)
        {
            GameObject spawnedMarble = Instantiate(prefabToSpawn, targetSlot.spawnPoint.position, Quaternion.identity);
            
            // --- BẮT ĐẦU CODE ĐƯỢC THÊM ---
            spawnedMarblesList.Add(spawnedMarble); // Lưu lại tham chiếu của bi để xóa sau này
            // --- KẾT THÚC CODE ĐƯỢC THÊM ---

            Collider2D col = spawnedMarble.GetComponent<Collider2D>();
            if (col != null)
            {
                PhysicsMaterial2D noBounceMat = new PhysicsMaterial2D("NoBounce");
                noBounceMat.bounciness = 0f; 
                noBounceMat.friction = 0.4f; 
                
                col.sharedMaterial = noBounceMat;
            }

            currentCollectedMarbles++;
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
            // --- BẮT ĐẦU CODE ĐƯỢC SỬA ---
            Debug.Log("Đã thu thập đủ bi! Đang chuyển khu vực màn chơi...");
            StartCoroutine(TransitionToNextLevelCoroutine());
            // --- KẾT THÚC CODE ĐƯỢC SỬA ---
        }
    }

    // --- BẮT ĐẦU CODE ĐƯỢC THÊM ---
    private IEnumerator TransitionToNextLevelCoroutine()
    {
        // 1. Xóa toàn bộ các bi đang có trong container
        foreach (GameObject marble in spawnedMarblesList)
        {
            if (marble != null)
            {
                Destroy(marble);
            }
        }
        spawnedMarblesList.Clear();

        // 2. Di chuyển Main Camera một khoảng Y = -10
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Vector3 startPos = mainCam.transform.position;
            Vector3 targetPos = startPos + new Vector3(0, -10f, 0);
            float duration = 1.0f; // Thời gian di chuyển camera (1 giây)
            float elapsed = 0f;

            while (elapsed < duration)
            {
                mainCam.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            mainCam.transform.position = targetPos;
        }

        // 3. Gọi sự kiện báo hiệu camera đã tới nơi để kích hoạt việc spawn bóng ở màn mới
        onCameraMovedToNewLevel?.Invoke();
    }
    // --- KẾT THÚC CODE ĐƯỢC THÊM ---
}