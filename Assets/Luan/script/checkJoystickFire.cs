using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkJoystickFire : MonoBehaviour
{
    // Tốc độ tối thiểu để coi là di chuyển (có thể điều chỉnh cho độ nhạy mong muốn)
    public float speedThreshold = 0.01f;
    public CamFire scriptCamfire;
    public CameraLook scriptCamlook;
    // Vị trí của object ở khung hình ban đầu
    private Vector3 initialPosition;

   
    public Animator Player;
    void Start()
    {
        
        // Khởi tạo vị trí ban đầu của object
        initialPosition = transform.position;



    }

    void Update()
    {
        // Tính toán khoảng cách di chuyển giữa vị trí hiện tại và vị trí ban đầu
        float distanceMoved = Vector3.Distance(transform.position, initialPosition);

        // Kiểm tra nếu khoảng cách vượt quá ngưỡng để xác định object đã di chuyển
        if (distanceMoved > speedThreshold)
        {
            

            scriptCamfire.enabled = true;
            scriptCamlook.enabled = false;


            Player.SetBool("fireYEN", true);
          

        
        }
        else
        {
            scriptCamfire.enabled = false;
            scriptCamlook.enabled = true;


            Player.SetBool("fireDI", false);
            Player.SetBool("fireYEN", false);
        }
    }

    




}
