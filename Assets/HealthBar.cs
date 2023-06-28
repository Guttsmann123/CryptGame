using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public EnemyHealth enemy;
    public Image healthBar;

    void Update()
    {
        healthBar.fillAmount = enemy.currentHealth / enemy.maxHealth;
    }
}
