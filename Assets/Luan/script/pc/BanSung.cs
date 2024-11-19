using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BanSung : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Animator Player;
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>(); // Thêm LineRenderer
        lineRenderer.startWidth = 0.1f; // Độ rộng đầu
        lineRenderer.endWidth = 0.1f; // Độ rộng cuối
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.red; // Màu đỏ
        lineRenderer.endColor = Color.red; // Màu đỏ
        lineRenderer.positionCount = 2; // Số điểm
        lineRenderer.enabled = false; // Tắt ban đầu
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Kiểm tra nếu nhấn chuột trái
        {
            if (Input.GetKey(KeyCode.W))
            {
                Player.SetBool("fireDI", true);
            }
            else
            {
                Player.SetBool("fireYEN", true);
            }

            lineRenderer.enabled = true; // Bật LineRenderer
            Vector3 start = transform.position; // Vị trí bắt đầu (từ GameObject này)
            Vector3 end = start + transform.forward * 100; // Điểm kết thúc (100 đơn vị về phía trước)

            lineRenderer.SetPosition(0, start); // Điểm bắt đầu
            lineRenderer.SetPosition(1, end); // Điểm kết thúc
            
            // Tự động tắt LineRenderer sau một thời gian
            Invoke("DisableLaser", 0.1f); // Thay đổi thời gian nếu cần

            // Kiểm tra va chạm với Raycast
            RaycastHit hit;
            if (Physics.Raycast(start, transform.forward, out hit, 100)) // Bắn tia 100 đơn vị
            {
                if (hit.collider.CompareTag("Enemy")) // Nếu đối tượng có tag "Enemy"
                {
                    Debug.Log("Đã bắn trúng đối tượng Enemy!"); // Thông báo
                }
            }

        }
        else
        {
            Player.SetBool("fireDI", false);
            Player.SetBool("fireYEN", false);
        }

    }

    void DisableLaser()
    {
       
        lineRenderer.enabled = false; // Tắt LineRenderer
    }
}
