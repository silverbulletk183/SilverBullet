using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public cam duongdan;



    //  public RaycastHit hit;
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    //Effect
    public GameObject muzzleEffect;

    // Loading
    public float reloadTime;
    public int magazineSise, bulletsLeft;
    public bool isReloading;

   //Sound
   public enum WeaponModel
    {
        Pistol1911,
        M16
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        InitializeWeapon();
        bulletsLeft = magazineSise;
    }

    void Update()
    {
        HandleShootingInput();
        LoadWeapon();
        

    }

    // Kh?i t?o giá tr? cho v? khí
    private void InitializeWeapon()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }

    // X? lý input b?n súng
    private void HandleShootingInput()
    {
        isShooting = CheckShootingInput();

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    // Ki?m tra input t? ng??i ch?i
    private bool CheckShootingInput()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            return Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }
        return false;
    }

    // X? lý b?n súng
    private void FireWeapon()
    {
        if (bulletsLeft <= 0)
        {
            Debug.Log("Không còn ??n, không th? b?n ti?p!");
            bulletsLeft = bulletsPerBurst;
            return;
        }
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        readyToShoot = false;

        //if (SoundManager.Instance != null)
        //{
        //    SoundManager.Instance.shootingSound1911.Play();
        //}
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        FireBullet();

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        //SoundManager.Instance.reloadingSound1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSise;
        isReloading = false;
    }

    private void LoadWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSise && isReloading == false)
        {
            Reload();
        }
        //Auto reload
        if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
        {
           
            Reload();
        }

        if (AmmoManager.Instance.amoDisplay != null)
        {

            AmmoManager.Instance.amoDisplay.text = $"{bulletsLeft / bulletsPerBurst} / {magazineSise / bulletsPerBurst}";
        }

        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyManagizeSound1911.Play();
        }
    }

    // B?n m?t viên ??n
    private void FireBullet()
    {
        duongdan.ban();


    }


   
    // Tính toán h??ng b?n và ?? lan (spread)
   // public Vector3 CalculateDirectionAndSpread()
   // {
       // Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
      //
      //  Debug.DrawLine(ray.origin, hit.point, Color.red);

     //   Vector3 targetPoint;
      //  if (Physics.Raycast(ray, out hit))
     //   {
       //     targetPoint = hit.point;
     //   }
     //   else
     //   {
      //      targetPoint = ray.GetPoint(100);
       // }

       // Vector3 direction = targetPoint - bulletSpawn.position;

      //  float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
       // float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

      //  return direction + new Vector3(x, y, 0);
   // }

    // Reset l?i tr?ng thái cho phép b?n ti?p
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    // Hu? viên ??n sau th?i gian delay
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
