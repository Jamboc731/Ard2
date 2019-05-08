using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

    [SerializeField] LifeCount lc;

    //if an enemy hits the body then lose a life
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            Die(other.gameObject);
        }
    }

    private void Die(GameObject a)
    {
        lc.RemoveLife();
        Destroy(a);
    }
}
