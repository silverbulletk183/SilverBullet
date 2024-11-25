using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damInEnemy : MonoBehaviour
{
    public void Hit(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Xử lý khi bị trúng tia laser
        Debug.Log($"{gameObject.name} was hit by laser at {hitPoint}");

        // Thực hiện hành động khi bị trúng (Ví dụ: nhận sát thương)
        TakeDamage(10);  // Gọi hàm nhận sát thương (nếu có)

        // Có thể thêm hiệu ứng như thay đổi màu sắc, tạo hiệu ứng ánh sáng, v.v.
    }

    void TakeDamage(int damage)
    {
        // Logic xử lý nhận sát thương, ví dụ giảm máu
        Debug.Log($"{gameObject.name} took {damage} damage.");
    }
}
