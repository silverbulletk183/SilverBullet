using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData/Guns/GunData", menuName = "SilverBullet/Gun")]
public class GunSO : ScriptableObject
{
    public string gunName;
    public string description;
    public int damage;
    public float fireRate;
    public float range;
    public int magazineSize;
    public float reloadTime;
    public Sprite gunSprite;
    public AudioClip shootSound;
    public AudioClip reloadSound;
}
