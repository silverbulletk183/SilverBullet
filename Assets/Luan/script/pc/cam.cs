using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    public Transform player; // Tham chiếu đến đối tượng nhân vật
    public float sensitivity = 5f; // Độ nhạy khi di chuyển chuột
    private float xRotation = 0f; // Góc xoay theo trục X

    void Start()
    {
        // Khóa con trỏ chuột để ngắm ời chơi không nhìn thấy con trỏ
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Lấy thông tin di chuyển của chuột
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;


        // Tính toán góc xoay theo trục X (lên xuống) và giới hạn góc
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 40f);

        // Áp dụng góc xoay cho camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay nhân vật theo trục Y (trái phải)
        player.Rotate(Vector3.up * mouseX);
    }
}
