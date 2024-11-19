using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoutManager : MonoBehaviour
{
    public Button logoutButton;

    void Start()
    {
        // Gán sự kiện cho nút đăng xuất
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    void OnLogoutButtonClicked()
    {
        // Chuyển về Scene A
        SceneManager.LoadScene("DNDK"); // Đảm bảo "SceneA" là tên chính xác của Scene bạn muốn chuyển đến
    }
}