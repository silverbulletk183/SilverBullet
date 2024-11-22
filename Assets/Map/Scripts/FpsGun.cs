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
    // Number Bulllet
    [SerializeField]
    private Text shotCounterText;
    private int shotCount = 0;

    private float timer;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        timer = 0.0f;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime;
        bool shooting = CrossPlatformInputManager.GetButton("Fire1");
        if (shooting && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
        animator.SetBool("Firing", shooting);
    }

    /// <summary>
    /// Shoot once, this also triggers effects and applies damage locally.
    /// </summary>
    void Shoot()
    {
        timer = 0.0f;
        shotCount++;
        UpdateShotCounterUI();

        gunLine.enabled = true;
        StartCoroutine(DisableShootingEffect());
        if (gunParticles.isPlaying)
        {
            gunParticles.Stop();
        }
        gunParticles.Play();

        // Raycasting for hit detection
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

        // Trigger third-person gun effects (if applicable)
        tpsGun?.TriggerShootEffect();
    }

    /// <summary>
    /// Instantiates the impact effect at the hit point.
    /// </summary>
    private void InstantiateImpactEffect(string effectName, Vector3 position, Vector3 normal)
    {
        GameObject impactEffect = Resources.Load<GameObject>(effectName);
        if (impactEffect != null)
        {
            Instantiate(impactEffect, position, Quaternion.LookRotation(normal));
        }
    }

    /// <summary>
    /// Coroutine function to disable shooting effect.
    /// </summary>
    public IEnumerator DisableShootingEffect()
    {
        yield return new WaitForSeconds(0.05f);
        gunLine.enabled = false;
    }

    private void UpdateShotCounterUI()
    {
        if (shotCounterText != null)
        {
            shotCounterText.text = shotCount.ToString();
        }
    }
}
