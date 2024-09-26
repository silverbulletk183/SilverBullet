using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 5f; 
    public Text deathMessage;
    public float delayBeforeReload = 2f;

    void Start()
    {
        health = maxHealth;
        deathMessage.gameObject.SetActive(false); // ?n th�ng b�o ch?t ban ??u
    }

    void Update()
    {
        // C?p nh?t gi� tr? c?a easeHealthSlider li�n t?c m?i khung h�nh
        if (easeHealthSlider.value != health)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed * Time.deltaTime);
            // D�ng Time.deltaTime ?? ??m b?o t?c ?? c?p nh?t theo th?i gian th?c
        }

        // C?p nh?t gi� tr? c?a healthSlider ngay l?p t?c
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        // Ki?m tra n?u m�u v? 0
        if (health <= 0 && !deathMessage.gameObject.activeSelf)
        {
            StartCoroutine(HandleDeath()); // G?i h�m x? l� khi ch?t
        }

        // Gi?m m�u khi nh?n ph�m M
        if (Input.GetKeyDown(KeyCode.M))
        {
            takeDamage(10);
        }
    }

   
    void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0; 
        }
    }

    
    IEnumerator HandleDeath()
    {
        deathMessage.gameObject.SetActive(true); 
        yield return new WaitForSeconds(delayBeforeReload); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
