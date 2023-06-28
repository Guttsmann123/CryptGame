using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // make sure your enemies have the tag "Enemy"
        {
            Destroy(other.gameObject);
        }
    }
}