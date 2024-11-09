using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkJoystickFire : MonoBehaviour
{
    // Tốc độ tối thiểu để coi là di chuyển (có thể điều chỉnh cho độ nhạy mong muốn)
    public float speedThreshold = 0.01f;
    public CamFire scriptCamfire;
    public CameraLook scriptCamlook;
    // Vị trí của object ở khung hình trước
    private Vector3 lastPosition;

    void Start()
    {
        // Khởi tạo vị trí ban đầu của object
        lastPosition = transform.position;
    }

    void Update()
    {
        // Tính toán khoảng cách di chuyển giữa khung hình hiện tại và khung hình trước
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // Kiểm tra nếu khoảng cách vượt quá ngưỡng để xác định object đang di chuyển
        if (distanceMoved > speedThreshold)
        {
            scriptCamfire.enabled = true;
            scriptCamlook.enabled = false;
        }
        else
        {
            scriptCamfire.enabled = false;
            scriptCamlook.enabled = true;
        }

        // Cập nhật vị trí cuối cùng
        lastPosition = transform.position;
    }
}
