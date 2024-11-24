using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleWeaponUI();
    }

    private void HandleWeaponUI()
    {
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
