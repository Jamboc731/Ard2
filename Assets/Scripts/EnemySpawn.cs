using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] spawns;
    float delay = 4;

	// spawn enemies at a decreasing interval at 1 of 3 spawn points
	void Start () {
        StartCoroutine(spawning());
	}
	
    IEnumerator spawning()
    {
        while (true)
        {
            Instantiate(enemy, spawns[Random.Range(0, 3)].position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
            delay -= Time.deltaTime;
        }
    }

}
