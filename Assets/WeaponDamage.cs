using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 20f;

    void OnTriggerEnter(Collider collider) // Change Collision to Collider
    {
        Debug.Log("Weapon collided with: " + collider.gameObject.name);
        EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>(); // Change collision to collider
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
