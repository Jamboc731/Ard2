using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float speed = 10;

    // mmove the enemy towards the player
    void Update()
    {
        transform.LookAt(GameObject.Find("Player").transform);
        transform.Translate(transform.forward * -speed * Time.deltaTime);
    }
}
