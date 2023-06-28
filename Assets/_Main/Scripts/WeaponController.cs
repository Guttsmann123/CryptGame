using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Staff;
    public bool canAttack = true;
    public float attackCooldown = 1.0f;
    private Coroutine cooldownCoroutine;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                StaffAttack();
            }
        }
    }

    public void StaffAttack()
    {
        canAttack = false;
        Animator anim = Staff.GetComponent<Animator>();
        anim.SetTrigger("Shoot");

        if (cooldownCoroutine == null)
        {
            cooldownCoroutine = StartCoroutine(ResetAttackCooldown());
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        cooldownCoroutine = null; // Reset the coroutine reference
    }
}
