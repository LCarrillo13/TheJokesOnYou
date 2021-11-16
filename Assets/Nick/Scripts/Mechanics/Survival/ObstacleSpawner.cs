using UnityEngine;
using System.Collections.Generic;
using Mirror;

namespace Networking
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] GameObject obstaclePrefab;
        [SerializeField] List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] float startDelay, interval;
        bool startedSpawning;

        void Update()
        {
            if (!CustomNetworkManager.Instance.canMove) return;

            if (!startedSpawning)
            {
                InvokeRepeating(nameof(SpawnObstacle), startDelay, interval);
                startedSpawning = true;
            }
        }

        void SpawnObstacle()
        {
            GameObject obstacle = Instantiate(obstaclePrefab, RandomSpawnPoint());
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
}
