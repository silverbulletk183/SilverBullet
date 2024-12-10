using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;


public class PlayerUI : MonoBehaviour
{
    [Space]
    [Header("Weapon")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image crossHairImage;

    private PlayerController playerController;
    public static PlayerUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Kiểm tra xem NetworkManager có sẵn không và tìm PlayerController của client hiện tại.

    }
    public void f(){

        if (NetworkManager.Singleton != null)
        {
            // Kiểm tra quyền sở hữu của client hiện tại

            foreach (var obj in FindObjectsOfType<PlayerController>())
            {
                if (obj.GetComponent<NetworkObject>().IsOwner)
                {
                    playerController = obj;
                    break;
                }
            }

          

        }

    }


    void Update()
    {
        // Cập nhật UI vũ khí mỗi frame
        if (playerController == null)
        {
            f();
        }
        else
        {
            UpdateWeaponUI(playerController.CurrentWeapon());
        }
    }

    private void UpdateWeaponUI(WeaponBase currentWeapon)
    {
        if (currentWeapon == null)
        {
            // Nếu không có vũ khí, ẩn UI liên quan
            crossHairImage.enabled = false;
            weaponImage.enabled = false;
            ammoText.text = "No Weapon";
            return;
        }

        // Hiển thị UI khi có vũ khí
        crossHairImage.enabled = true;
        weaponImage.enabled = true;

        // Cập nhật các thành phần UI
        crossHairImage.sprite = currentWeapon.weaponData.crossHairSprite;
        weaponImage.sprite = currentWeapon.weaponData.weaponIconSprite;
        weaponImage.rectTransform.sizeDelta = new Vector2(
            currentWeapon.weaponData.weaponIconImageWidth,
            currentWeapon.weaponData.weaponIconImageHeight
        );

        ammoText.text = $"{currentWeapon.currentAmmo} / {currentWeapon.totalAmmo}";
    }
}
