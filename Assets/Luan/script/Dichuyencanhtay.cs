using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dichuyencanhtay : MonoBehaviour
{
    public Transform follower; // Đối tượng sẽ di chuyển theo

    public float rotationSpeed = 20f; // Tốc độ quay

    void Update()
    {
        // Quay quanh trục X
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * rotationAmount);

        // Cập nhật rotation Z của follower theo rotation X của rotating object
        if (follower != null)
        {
            // Lấy rotation X của rotating object
            float rotationX = transform.rotation.eulerAngles.x;

            // Chuyển đổi rotation X thành rotation Z cho follower
            follower.rotation = Quaternion.Euler(0, rotationX, 0);
        }
    }
}
