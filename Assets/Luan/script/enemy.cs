using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float detectionRange = 100f; // Tầm xa của raycast
    public Transform raycastOrigin; // Điểm xuất phát của Raycast, nếu null thì mặc định là vị trí Enemy

    void Update()
    {
        RaycastHit hit;
        Vector3 start = raycastOrigin != null ? raycastOrigin.position : transform.position; // Lấy vị trí bắt đầu
        Vector3 direction = transform.forward; // Hướng bắn tia (phía trước Enemy)

        // Bắn raycast để kiểm tra va chạm
        if (Physics.Raycast(start, direction, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Player")) // Kiểm tra nếu đối tượng va chạm là Player
            {
                Debug.Log("Phát hiện Player!");
                // Thêm hành động khác nếu cần, ví dụ: tấn công
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Vẽ raycast để dễ dàng kiểm tra trong Editor
        Gizmos.color = Color.red;
        Vector3 start = raycastOrigin != null ? raycastOrigin.position : transform.position;
        Gizmos.DrawRay(start, transform.forward * detectionRange);
    }
}
