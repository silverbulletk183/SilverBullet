using Assets.Script.Vinh.UI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class HealthManager : NetworkBehaviour
{
    public static HealthManager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private float maxHealth = 100f; // Lượng máu tối đa.

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(); // Biến mạng để đồng bộ máu giữa host và client.
    private ThanhMau thanhMau;
    // private NetworkVariable<bool> repostDeath = new NetworkVariable<bool>();
    private bool repostDeath;
    private void Start()
    {
        thanhMau = GameObject.Find("CanvasRANGE").GetComponent<ThanhMau>();
    }

    public override void OnNetworkSpawn()
    {
       
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
            repostDeath= true;
            // Khi host khởi tạo, đặt máu hiện tại bằng máu tối đa.
        }

        currentHealth.OnValueChanged += OnHealthChanged;

        
    }

   

    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthUI(newHealth);
        // Cập nhật giao diện người chơi khi máu thay đổi.
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer) return; // Chỉ server mới được phép thay đổi giá trị máu.

        currentHealth.Value = Mathf.Max(currentHealth.Value - damage, 0); // Trừ máu, đảm bảo không âm.

        if (currentHealth.Value <= 0&& repostDeath==true)
        {
                HandleDeath();
                repostDeath = false;
               
            // Xử lý khi nhân vật chết.
        }
    }

    private void HandleDeath()
    {
        
            ulong ownerClientId = OwnerClientId;
            // Xử lý logic khi nhân vật chết (ví dụ: hồi sinh, vô hiệu hóa input).
            TeamDeathManager.Instance.ReportPlayerDeath(ownerClientId % 2 == 0 ? "A" : "B");// In thông báo nhân vật đã chết.
        
       
        
    }
    private void ResetHealth()
    {
        currentHealth.Value = maxHealth;
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetHealthServerRpc()
    {
        ResetHealth();
    }
   /* private void ResetRepost()
    {
        repostDeath.Value = true;

    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetRepostServerRpc()
    {
       ResetRepost();
    }*/

    private void UpdateHealthUI(float health)
    {
        if (IsOwner)
        {
            Debug.Log("lllllll");
            repostDeath= true;
            thanhMau.capnhatthanhmau(currentHealth.Value, maxHealth);// Cập nhật thanh máu cho người chơi sở hữu.
            Debug.Log("lllllll"+repostDeath);
        }
    }
}
