using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject healthBarUI;
    public Slider slider;


    void Start()
    {
        currentHealth = maxHealth;
        slider.value = CalculateHealth();

    }


    void Update ()
    {
        slider.value = CalculateHealth();
        if (currentHealth < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    
    float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            Debug.Log("Taking damage. Current health: " + currentHealth);
            currentHealth = 0;
            // You could put code here to destroy the enemy or play a death animation
        }
    }
}
