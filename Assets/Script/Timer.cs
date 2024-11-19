using TMPro;
using UnityEngine;
using UnityEngine.UI;  // Dùng nếu bạn sử dụng UI Text. Nếu dùng TextMeshPro thì thay thế bằng TextMeshPro namespace.

public class Timer : MonoBehaviour
{
    // Các biến cho minutes và seconds
    private float timeRemaining = 0f;
    private int minutes;
    private int seconds;

    // Tham chiếu đến Text UI để hiển thị thời gian
    public TextMeshProUGUI timeText;

    void Start()
    {
        // Đặt thời gian bắt đầu (ví dụ: 0 phút, 0 giây)
        timeRemaining = 0f;
    }

    void Update()
    {
        // Tăng thời gian mỗi frame
        timeRemaining += Time.deltaTime;

        // Tính toán phút và giây
        minutes = Mathf.FloorToInt(timeRemaining / 60); // Lấy số phút
        seconds = Mathf.FloorToInt(timeRemaining % 60); // Lấy số giây

        // Cập nhật hiển thị thời gian (phút:giây)
        timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }
}
