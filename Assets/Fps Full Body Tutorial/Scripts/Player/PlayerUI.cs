using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
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


    // Start is called before the first frame update
    void Start()
    {
        // GameObject player = gameObject. 
        if (NetworkManager.Singleton != null)
        {
            foreach (var obj in FindObjectsOfType<PlayerController>())
            {
                // Kiểm tra quyền sở hữu
                if (obj.GetComponent<NetworkObject>().IsOwner)
                {
                    playerController = obj;
                    break;
                }
            }

            if (playerController == null)
            {
                Debug.Log("Không tìm thấy PlayerController của chính chủ sở hữu!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       HandleWeaponUI();
    }

    private void HandleWeaponUI()
    {
        if (playerController == null) return;
        // get the current weapon reference.
        WeaponBase currentWeapon = playerController.CurrentWeapon();

        // set the cross hair and weapon images.
        crossHairImage.sprite = currentWeapon.weaponData.crossHairSprite;
        weaponImage.sprite = currentWeapon.weaponData.weaponIconSprite;
        // SIZE of the image
        weaponImage.rectTransform.sizeDelta = new Vector2(currentWeapon.weaponData.weaponIconImageWidth, currentWeapon.weaponData.weaponIconImageHeight);

        // set the ammoText.
        ammoText.text = currentWeapon.currentAmmo + " / " + currentWeapon.totalAmmo.ToString();
    }
}
