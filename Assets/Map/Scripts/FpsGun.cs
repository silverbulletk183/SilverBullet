using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using UnityEngine.UI;

public class FpsGun : MonoBehaviour
{
    [SerializeField]
    private int damagePerShot = 20;
    [SerializeField]
    private float timeBetweenBullets = 0.2f;
    [SerializeField]
    private float weaponRange = 100.0f;
    [SerializeField]
    private TpsGun tpsGun;
    [SerializeField]
    private ParticleSystem gunParticles;
    [SerializeField]
    private LineRenderer gunLine;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Camera raycastCamera;
    [SerializeField]
    private Text shotCounterText;
    [SerializeField]
    private Text ammoCounterText;

    [SerializeField]
    private int maxAmmo = 30; // S? ??n t?i ?a
    [SerializeField]
    private int ammoCapacity = 120; // S? ??n t?ng c?ng ng??i ch?i có
    [SerializeField]
    private int reloadAmount = 30; // S? ??n m?i l?n n?p

    private int currentAmmo;
    private int shotCount = 0;
    private float timer;

    void Start()
    {
        timer = 0.0f;
        currentAmmo = maxAmmo; // B?t ??u v?i b?ng ??n ??y
        UpdateUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        bool shooting = CrossPlatformInputManager.GetButton("Fire1");

        if (shooting && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
        }

        /*if (CrossPlatformInputManager.GetButtonDown("Reload") || Input.GetKeyDown(KeyCode.G))
        {
            Reload();
        }*/
        if (Input.GetKeyDown(KeyCode.G))
        {
            Reload();
        }
       // animator.SetBool("Firing", shooting);
    }

    void Shoot()
    {
        timer = 0.0f;

        // Tr? ??n hi?n có
        currentAmmo--;
        shotCount++;
        UpdateUI();

        gunLine.enabled = true;
        StartCoroutine(DisableShootingEffect());
        if (gunParticles.isPlaying)
        {
            gunParticles.Stop();
        }
        gunParticles.Play();

        RaycastHit shootHit;
        Ray shootRay = raycastCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        if (Physics.Raycast(shootRay, out shootHit, weaponRange, LayerMask.GetMask("Shootable")))
        {
            string hitTag = shootHit.transform.gameObject.tag;
            switch (hitTag)
            {
                case "Player":
                    var hitPlayer = shootHit.collider.GetComponent<PlayerHealth>();
                    if (hitPlayer != null)
                    {
                        hitPlayer.TakeDamage(damagePerShot);
                    }
                    InstantiateImpactEffect("impactFlesh", shootHit.point, shootHit.normal);
                    break;

                default:
                    InstantiateImpactEffect("impact" + hitTag, shootHit.point, shootHit.normal);
                    break;
            }
        }

        tpsGun?.TriggerShootEffect();
    }

    private void Reload()
    {
        if (ammoCapacity > 0 && currentAmmo < maxAmmo)
        {
            int neededAmmo = maxAmmo - currentAmmo; // S? ??n c?n ?? ??y b?ng
            int ammoToReload = Mathf.Min(neededAmmo, ammoCapacity);

            currentAmmo += ammoToReload;
            ammoCapacity -= ammoToReload;

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (shotCounterText != null)
        {
            shotCounterText.text =  shotCount.ToString();
        }

        if (ammoCounterText != null)
        {
            ammoCounterText.text =  currentAmmo.ToString() + " / " + ammoCapacity.ToString();
        }
    }

    public IEnumerator DisableShootingEffect()
    {
        yield return new WaitForSeconds(0.05f);
        gunLine.enabled = false;
    }

    private void InstantiateImpactEffect(string effectName, Vector3 position, Vector3 normal)
    {
        GameObject impactEffect = Resources.Load<GameObject>(effectName);
        if (impactEffect != null)
        {
            Instantiate(impactEffect, position, Quaternion.LookRotation(normal));
        }
    }
}
