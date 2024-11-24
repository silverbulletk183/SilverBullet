using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(FirstPersonController))]
public class PlayerHealth : MonoBehaviour
{
    public delegate void Respawn(float time);
    public delegate void AddMessage(string message);
    public event Respawn RespawnEvent;
    public event AddMessage AddMessageEvent;

    [SerializeField]
    private int startingHealth = 100;
    [SerializeField]
    private float sinkSpeed = 0.12f;
    [SerializeField]
    private float sinkTime = 2.5f;
    [SerializeField]
    private float respawnTime = 8.0f;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private AudioClip hurtClip;
    [SerializeField]
    private AudioSource playerAudio;
    [SerializeField]
    private float flashSpeed = 2f;
    [SerializeField]
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [SerializeField]
    private Animator animator;

    private FirstPersonController fpController;
    private IKControl ikControl;
    private Slider healthSlider;
    private Image damageImage;
    private int currentHealth;
    private bool isDead;
    private bool isSinking;
    private bool damaged;
    private Vector3 sinkingTargetPosition;

    void Start()
    {
        fpController = GetComponent<FirstPersonController>();
        ikControl = GetComponentInChildren<IKControl>();
        damageImage = GameObject.FindGameObjectWithTag("Screen").transform.Find("DamageImage").GetComponent<Image>();
        healthSlider = GameObject.FindGameObjectWithTag("Screen").GetComponentInChildren<Slider>();
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;

        damaged = false;
        isDead = false;
        isSinking = false;
    }

    void Update()
    {
        if (damaged)
        {
            damaged = false;
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        if (isSinking)
        {
            transform.position = Vector3.MoveTowards(transform.position, sinkingTargetPosition, sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        damaged = true;
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }

        healthSlider.value = currentHealth;
        animator.SetTrigger("IsHurt");
        playerAudio.clip = hurtClip;
        playerAudio.Play();
    }

    private void Death()
    {
        isDead = true;
        ikControl.enabled = false;

        fpController.enabled = false;
        animator.SetTrigger("IsDead");

        AddMessageEvent?.Invoke("Player has died!");
        RespawnEvent?.Invoke(respawnTime);

        StartCoroutine(DestroyPlayer(respawnTime));

        playerAudio.clip = deathClip;
        playerAudio.Play();

        StartCoroutine(StartSinking(sinkTime));
    }

    IEnumerator DestroyPlayer(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }

    IEnumerator StartSinking(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        // Tính toán v? trí ?ích ?? chìm
        sinkingTargetPosition = transform.position + Vector3.down * sinkSpeed * sinkTime;

        isSinking = true;
    }
}
