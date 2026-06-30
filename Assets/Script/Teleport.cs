using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Bắt đầu code sửa
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Obs_1.1"))
        {
            GameObject container = GameObject.Find("Marble_container");
            if (container != null)
            {
                // Dịch chuyển lập tức khi viền collider vừa chạm
                transform.position = container.transform.position;
            }
        }
    }
    // Kết thúc code sửa
}