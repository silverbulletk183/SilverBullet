using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGunClass
{
    Pistol,
    Rifle,
    Shotgun,
    Sniper,
    SMG
}
[CreateAssetMenu(fileName = "GameData/Guns/GunData", menuName = "SilverBullet/Gun")]
public class GunSO : ScriptableObject
{
    public string gunName;
    public string description;
    public EGunClass gunClass;
    public int damage;
    public float fireRate;
    public float range;
    public int magazineSize;
    public float reloadTime;
    public Sprite gunSprite;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public static GunSO createGunFromJson(string json)
    {
        GunSO gun = ScriptableObject.CreateInstance<GunSO>();
        JsonUtility.FromJsonOverwrite(json, gun);
        return gun;
    }
}
