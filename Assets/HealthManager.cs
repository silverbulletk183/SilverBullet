using Assets.Script.Vinh.UI.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class HealthManager : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Lượng máu tối đa.

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(); // Biến mạng để đồng bộ máu giữa host và client.
    private ThanhMau thanhMau;
    private void Start()
    {
        thanhMau = GameObject.Find("CanvasRANGE").GetComponent<ThanhMau>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth; // Khi host khởi tạo, đặt máu hiện tại bằng máu tối đa.
        }

        currentHealth.OnValueChanged += OnHealthChanged; // Lắng nghe sự thay đổi giá trị máu.
    }

    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthUI(newHealth); // Cập nhật giao diện người chơi khi máu thay đổi.
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer) return; // Chỉ server mới được phép thay đổi giá trị máu.

        currentHealth.Value = Mathf.Max(currentHealth.Value - damage, 0); // Trừ máu, đảm bảo không âm.

        if (currentHealth.Value <= 0)
        {
            HandleDeath(); // Xử lý khi nhân vật chết.
        }
    }

    private void HandleDeath()
    {
        // Xử lý logic khi nhân vật chết (ví dụ: hồi sinh, vô hiệu hóa input).
        Debug.Log($"{gameObject.name} has died."); // In thông báo nhân vật đã chết.
    }

    private void UpdateHealthUI(float health)
    {
        if (IsOwner)
        {
            thanhMau.capnhatthanhmau(currentHealth.Value, maxHealth);// Cập nhật thanh máu cho người chơi sở hữu.
        }
    }
}
