using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm th? vi?n ?? t?i l?i màn ch?i
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    public Text deathMessage; // Thêm m?t ??i t??ng Text ?? hi?n th? thông báo
    public float delayBeforeReload = 2f; // Th?i gian ch? tr??c khi t?i l?i màn ch?i

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        deathMessage.gameObject.SetActive(false); // ?n thông báo ch?t ban ??u
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

        // Thanh máu gi?m t? t?
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        // Ki?m tra n?u máu v? 0
        if (health <= 0 && !deathMessage.gameObject.activeSelf)
        {
            StartCoroutine(HandleDeath()); // G?i hàm x? lý khi ch?t
        }
    }

    // Hàm x? lý vi?c gi?m máu
    void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0; // ??m b?o máu không âm
        }
    }

    // Hàm x? lý khi nhân v?t ch?t
    IEnumerator HandleDeath()
    {
        deathMessage.gameObject.SetActive(true); // Hi?n th? thông báo ch?t
        yield return new WaitForSeconds(delayBeforeReload); // Ch? m?t th?i gian tr??c khi t?i l?i màn ch?i
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // T?i l?i màn ch?i hi?n t?i
    }
}
