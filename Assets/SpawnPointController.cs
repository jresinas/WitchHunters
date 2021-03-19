using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour {
    public GameObject[] enemyPrefab;
    // Probability (0,1) per second to spawn
    [Range(0f, 1f)] public float spawnFrecuency;
    // Minimum number of spawns
    public int minNumber = 0;
    // Maximum number of spawns
    public int maxNumber = 1;


    // Start is called before the first frame update
    void Start() {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator Timer() {
        int number = Random.Range(minNumber, maxNumber + 1);
        if (Random.Range(0f, 1f) <= spawnFrecuency) {
            for (int i = 0; i < number; i++) Instantiate(GetEnemy(), transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer());
    }

    GameObject GetEnemy() {
        if (enemyPrefab.Length >= 1) {
            return enemyPrefab[Random.Range(0, enemyPrefab.Length)];
        } else {
            return null;
        }
    }
}
