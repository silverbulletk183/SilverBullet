using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFire : MonoBehaviour
{
    // Các tham số cần thiết
    public Joystick joystick;  // Tham chiếu đến joystick
    public Camera playerCamera; // Camera của người chơi
    public float rotationSpeed = 5f; // Tốc độ xoay của camera và nhân vật

    private float pitch = 0f; // Góc xoay dọc (pitch)
    private float yaw = 0f; // Góc xoay ngang (yaw)

    public Transform playerTransform; // Tham chiếu đến transform của nhân vật

    void Start()
    {
       
    }

    void Update()
    {
        // Lấy input từ joystick
        float horizontalInput = joystick.Horizontal; // Xoay nhân vật theo trục ngang (yaw)
        float verticalInput = joystick.Vertical; // Xoay camera theo trục dọc (pitch)

        // Cập nhật góc yaw (xoay nhân vật) dựa trên input ngang của joystick
        yaw += horizontalInput * rotationSpeed;

        // Cập nhật góc pitch (xoay camera) dựa trên input dọc của joystick
        pitch -= verticalInput * rotationSpeed;

        // Giới hạn pitch để tránh camera xoay quá cao hoặc quá thấp (góc giữa -90 và 90 độ)
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Áp dụng xoay cho camera (chỉ xoay dọc)
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Áp dụng xoay cho nhân vật (chỉ xoay ngang)
        playerTransform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

}
