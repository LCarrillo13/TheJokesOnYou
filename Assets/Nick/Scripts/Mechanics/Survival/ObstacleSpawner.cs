using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab; 
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] float startDelay, interval;

    void Start() => InvokeRepeating(nameof(SpawnObstacle), startDelay, interval);

    void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, RandomSpawnPoint());
        NetworkServer.Spawn(obstaclePrefab);
    }


    // returns a random spawn point from a list
    Transform RandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];
        return spawnPoint;
    }
}
