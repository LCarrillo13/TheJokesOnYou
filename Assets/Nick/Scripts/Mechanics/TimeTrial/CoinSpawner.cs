using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{ 
    [SerializeField] GameObject coinPrefab; 
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] float startDelay, interval;

    void Start() => InvokeRepeating(nameof(SpawnObstacle), startDelay, interval);

    void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(coinPrefab, RandomSpawnPoint());
        NetworkServer.Spawn(obstacle);
    }


    // returns a random spawn point from a list
    Transform RandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];
        return spawnPoint;
    }
}
