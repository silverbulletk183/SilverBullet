using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponBase
{


    // Custom Shotgun Variables.
    [Space]
    [Header("Shotgun Custom")]

    [Space]
    [Header("Animation Names")]
    [SerializeField] private string loadBulletFromSide = "";
    [SerializeField] private string loadBulletFromBelowLoop = "";
    [SerializeField] private string loadBulletFromBelow = "";

    [Space]
    [Header("Animation Time")]
    [SerializeField] private float loadBulletFromSideAnimationTime;
    [SerializeField] private float loadBulletFromBelowAnimationTime;
    [SerializeField] private float loadBulletFromBelowLoopAnimationTime;

    [Space]
    [Header("Shotgun Audio Clips")]
    [SerializeField] private AudioClip pushBulletInAudioClip;

    // This helps with reloading.
    private int bulletsLoaded = 0;
    private int bulletsToLoad = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        // Call The Base classes update.
        base.Update();
    }

    /// <summary>
    /// Custom start reloading function, overrides the original one in the base class but still calls that one at the end,
    /// Instead of doing that functionality itself.
    /// </summary>
    public override void StartReloading()
    {
        // Helper Variables.
        int bulletsDifferenceFromMagSize = weaponData.magazineSize - currentAmmo;
        bool hasEnoughBulletsToLoadComplete = bulletsDifferenceFromMagSize <= totalAmmo ? true : false;

        // Check if we have enough bullets to load complete
        if (hasEnoughBulletsToLoadComplete == true)
        {
            // Set the bullets to load to the deference in bullets from the mag size.
            bulletsToLoad = bulletsDifferenceFromMagSize;
        }
        else
        {
            // Set the bullets to load to the total amount of ammo.
            bulletsToLoad = totalAmmo;
        }

        // Set the bullets loaded to 0.
        bulletsLoaded = 0;

        // Check if the mag isnt empty.
        if (currentAmmo > 0)
        {
            // Calculate the Time for the weapon to wait until it's able to shoot again.
            weaponData.reloadingAnimationTime = (loadBulletFromBelowLoopAnimationTime * (bulletsToLoad - 1)) + loadBulletFromBelowAnimationTime;

            // Play the load bullet from below animation.
            animator.Play(loadBulletFromBelow, 0, 0);
        }
        else
        {
            // Calculate the Time for the weapon to wait until it's able to shoot again.
            weaponData.reloadingAnimationTime = (loadBulletFromBelowLoopAnimationTime * (bulletsToLoad - 2)) + (loadBulletFromBelowAnimationTime + loadBulletFromSideAnimationTime);

            // Play the animation for Loading a bullet from the side.
            animator.Play(loadBulletFromSide, 0, 0);
        }

        // Base Reload, Instead of doing the base functionality in here, Just call the Base Function.
        base.StartReloading();
    }

    /// <summary>
    /// Called When the Animation that loads the bullet from the side is finished using an Animation Event.
    /// </summary>
    private void OnFinishedLoadBulletFromSide()
    {
        // Manage Bullets Loaded & Ammo.
        bulletsLoaded += 1;
        currentAmmo += 1;
        totalAmmo -= 1;

        // Check if we still need to load Bullets.
        if (bulletsLoaded < bulletsToLoad)
        {
            // Play the Load Bullet from below animation.
            animator.Play(loadBulletFromBelow, 0, 0);
        }
    }

    /// <summary>
    /// Called When the Animation that loads the bullet from below is at the looping point using an Animation Event.
    /// </summary>
    private void OnFinishedLoadBulletFromBelow()
    {
        // Manage Bullets Loaded & Ammo.
        bulletsLoaded += 1;
        currentAmmo += 1;
        totalAmmo -= 1;

        // Check if we still need to load Bullets.
        if (bulletsLoaded < bulletsToLoad)
        {
            // Load Another Bullet in
            animator.Play(loadBulletFromBelowLoop, 0, 0);
        }
    }

    // Plays the bullet click audio clip.
    private void PlayPushBulletInAudioClip()
    {
        // Play the Bullet in audio clip.
        AudioSource.PlayClipAtPoint(pushBulletInAudioClip, transform.position, 1.0f);
    }
}
