using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
   public float raycastDistance = 10f; // Khoảng cách tối đa của raycast
   public RaycastHit hit;
   public void ban()
    {
        // Tạo một ray bắt đầu từ vị trí của GameObject và hướng theo trục forward
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);

        Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);


        // Kiểm tra va chạm
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Debug.Log("Va chạm với: " + hit.collider.gameObject.name);

            // Thực hiện các hành động khi có va chạm
            // Ví dụ:
            // - Áp dụng lực lên đối tượng bị va chạm
            // - Phá hủy đối tượng bị va chạm
            // - Trigger một sự kiện
        }
    }
}
