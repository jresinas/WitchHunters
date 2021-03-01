using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour {
    public GameObject[] enemyPrefab;
    // Probability (0,1) per second to spawn
    [Range(0f, 1f)] public float spawnFrecuency;
    // Minimum time between two spawns
    public float minFrecuency = 0;
    // Maximum time between two spawns
    public float maxFrecuency = 0;


    // Start is called before the first frame update
    void Start() {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator Timer() {
        if (Random.Range(0f, 1f) <= spawnFrecuency) Instantiate(GetEnemy(), transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer());
    }

    GameObject GetEnemy() {
        if (enemyPrefab.Length >= 1) {
            return enemyPrefab[Random.Range(0, enemyPrefab.Length - 1)];
        } else {
            return null;
        }
    }
}
