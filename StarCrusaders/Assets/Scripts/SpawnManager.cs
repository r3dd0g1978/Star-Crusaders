using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float[] spawnRates;
    [SerializeField] int[] startingSpawnCount;
    [SerializeField] int waveCount;
    [SerializeField] Transform[] spawnPoints;

    int currentSpawnCount;
    
    void Start()
    {
        currentSpawnCount = startingSpawnCount[0];
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < waveCount; i++)
        {
            currentSpawnCount = startingSpawnCount[i];

            while (currentSpawnCount > 0)
            {
                int randomSpawnPosIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefabs[i], spawnPoints[randomSpawnPosIndex].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnRates[i]);
                currentSpawnCount--;
            }
        }
    }
}
