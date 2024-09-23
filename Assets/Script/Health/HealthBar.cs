using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Th�m th? vi?n ?? t?i l?i m�n ch?i
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    public Text deathMessage; // Th�m m?t ??i t??ng Text ?? hi?n th? th�ng b�o
    public float delayBeforeReload = 2f; // Th?i gian ch? tr??c khi t?i l?i m�n ch?i

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        deathMessage.gameObject.SetActive(false); // ?n th�ng b�o ch?t ban ??u
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            takeDamage(10);
        }

        // Thanh m�u gi?m t? t?
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        // Ki?m tra n?u m�u v? 0
        if (health <= 0 && !deathMessage.gameObject.activeSelf)
        {
            StartCoroutine(HandleDeath()); // G?i h�m x? l� khi ch?t
        }
    }

    // H�m x? l� vi?c gi?m m�u
    void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0; // ??m b?o m�u kh�ng �m
        }
    }

    // H�m x? l� khi nh�n v?t ch?t
    IEnumerator HandleDeath()
    {
        deathMessage.gameObject.SetActive(true); // Hi?n th? th�ng b�o ch?t
        yield return new WaitForSeconds(delayBeforeReload); // Ch? m?t th?i gian tr??c khi t?i l?i m�n ch?i
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // T?i l?i m�n ch?i hi?n t?i
    }
}
