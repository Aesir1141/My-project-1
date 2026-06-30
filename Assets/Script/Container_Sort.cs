using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MarbleSorter : MonoBehaviour
{
    // Bắt đầu code thêm
    public float spacing = 1.0f; // Khoảng cách giữa các quả bóng khi xếp

    void Start()
    {
        SortAndMoveMarbles();
    }

    public void SortAndMoveMarbles()
    {
        // Tìm container
        GameObject container = GameObject.Find("Marble_container");
        if (container == null) return;

        // Tìm tất cả các bóng (yêu cầu các object bóng phải được gán tag "Marble")
        GameObject[] marbles = GameObject.FindGameObjectsWithTag("Marble");

        // Nhóm bóng dựa vào Tên và Màu sắc, sau đó sắp xếp theo số lượng từ nhiều tới ít
        var sortedGroups = marbles
            .GroupBy(m => new
            {
                Name = m.name,
                Color = m.GetComponent<Renderer>() != null ? m.GetComponent<Renderer>().material.color : Color.white
            })
            .OrderByDescending(g => g.Count())
            .ToList();

        // Lấy tọa độ X bắt đầu từ vị trí của container để xếp từ trái sang phải
        float currentXPosition = container.transform.position.x;

        foreach (var group in sortedGroups)
        {
            foreach (GameObject marble in group)
            {
                // Di chuyển bóng vào trong container (set parent)
                marble.transform.SetParent(container.transform);

                // Đặt vị trí dàn đều từ trái sang phải dọc theo trục X
                marble.transform.position = new Vector3(
                    currentXPosition,
                    container.transform.position.y,
                    container.transform.position.z
                );

                // Tăng tọa độ X cho bóng tiếp theo
                currentXPosition += spacing;
            }
        }
    }
    // Kết thúc code thêm
}