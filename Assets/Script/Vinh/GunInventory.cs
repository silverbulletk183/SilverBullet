using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunInventory : MonoBehaviour
{
    public List<GunSO> guns; // Danh sách các khẩu súng
    private VisualElement gunInventoryElement;

    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        gunInventoryElement = uiDocument.rootVisualElement.Q<VisualElement>("GunInventory");

        PopulateGunInventory();
    }

    private void PopulateGunInventory()
    {
        foreach (var gun in guns)
        {
            var gunElement = new VisualElement();
            gunElement.AddToClassList("gun-element"); // Thêm class CSS

            // Tạo và thêm hình ảnh
            var icon = new Image { image = gun.gunSprite.texture };
            gunElement.Add(icon);

            // Thêm tên và mô tả
            var nameLabel = new Label(gun.gunName);
            gunElement.Add(nameLabel);

            var descriptionLabel = new Label(gun.description);
            gunElement.Add(descriptionLabel);

            var statsLabel = new Label($"Damage: {gun.damage}, Fire Rate: {gun.fireRate}, Magazine Size: {gun.magazineSize}");
            gunElement.Add(statsLabel);

            // Thêm phần tử súng vào kho súng
            gunInventoryElement.Add(gunElement);
        }
    }
}
