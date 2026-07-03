using System.Collections;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Tooltip("Danh sách các Prefab của bi cần spawn. Bi sẽ spawn tuần tự theo danh sách này.")]
    public GameObject[] marblePrefabs; 
    
    [Tooltip("Danh sách các vị trí sẽ spawn bi")]
    public Transform[] spawnPoints;

    [Tooltip("Tổng số bi bạn muốn sinh ra ở màn này")]
    public int totalMarblesToSpawn = 10;

    // --- BẮT ĐẦU CODE ĐƯỢC SỬA ---
    [Tooltip("Độ trễ (giây) giữa mỗi lần rơi. Đặt số rất nhỏ (VD: 0.02) để spawn nhanh cho marble race.")]
    public float spawnDelay = 0.02f;
    // --- KẾT THÚC CODE ĐƯỢC SỬA ---

    // --- BẮT ĐẦU CODE ĐƯỢC THÊM ---
    [Tooltip("Khoảng cách phân tán ngẫu nhiên xung quanh điểm spawn để bi không bị lồng vào nhau.")]
    public float spawnOffset = 0.5f;
    // --- KẾT THÚC CODE ĐƯỢC THÊM ---

    public void SpawnNewMarbles()
    {
        if (marblePrefabs == null || marblePrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Chưa gán Prefab hoặc Spawn Points trong LevelSpawner!");
            return;
        }

        StartCoroutine(SpawnMarblesCoroutine());
    }

    private IEnumerator SpawnMarblesCoroutine()
    {
        for (int i = 0; i < totalMarblesToSpawn; i++)
        {
            int prefabIndex = i % marblePrefabs.Length;
            GameObject prefabToSpawn = marblePrefabs[prefabIndex];

            int pointIndex = i % spawnPoints.Length;
            Transform spawnPoint = spawnPoints[pointIndex];

            if (prefabToSpawn != null)
            {
                // --- BẮT ĐẦU CODE ĐƯỢC THÊM ---
                // Tạo một vị trí ngẫu nhiên trong vòng tròn bán kính spawnOffset
                Vector2 randomCircle = Random.insideUnitCircle * spawnOffset;
                Vector3 finalSpawnPosition = spawnPoint.position + new Vector3(randomCircle.x, randomCircle.y, 0f);
                
                Instantiate(prefabToSpawn, finalSpawnPosition, Quaternion.identity);
                // --- KẾT THÚC CODE ĐƯỢC THÊM ---
            }

            // --- BẮT ĐẦU CODE ĐƯỢC SỬA ---
            if (spawnDelay > 0)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
            // --- KẾT THÚC CODE ĐƯỢC SỬA ---
        }

        Debug.Log($"Đã spawn xong {totalMarblesToSpawn} viên bi!");
    }
}