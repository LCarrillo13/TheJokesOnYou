using UnityEngine;
using System.Collections.Generic;
using Mirror;
using System.Collections;

namespace Networking
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Obstacle Setup")]
        [SerializeField] GameObject obstaclePrefab;
        [SerializeField] List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] float startDelay, interval;
        bool startedSpawning;
        [Header("Timer")]
        [SerializeField] int count;
        public float changedSpeed;

        void Update()
        {
            if (!CustomNetworkManager.Instance.canMove) return;

            if (!startedSpawning)
            {
                StartCoroutine(Timer(60));
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

        public IEnumerator Timer(int seconds)
        {
            count = seconds;

            while (count > 0)
            {
                changedSpeed += 1;
                yield return new WaitForSeconds(1);
                count--;
            }
        }
    } 
}
