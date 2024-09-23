using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    

    public AudioClip P1911Shot;
    public AudioClip M16Shot;

    public AudioSource reloadingSound1911;
    public AudioSource reloadingSoundM16;

    public AudioSource emptyManagizeSound1911;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Debug.Log("SoundManager Instance set.");

        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.M16:
                ShootingChannel.PlayOneShot(M16Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.Play();
                break;
            case WeaponModel.M16:
                ShootingChannel.Play();
                break;
        }
    }
}
