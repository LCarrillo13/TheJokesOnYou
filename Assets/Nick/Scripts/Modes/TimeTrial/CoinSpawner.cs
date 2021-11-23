using Mirror;

using Networking;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class CoinSpawner : NetworkBehaviour
{ 
    [SerializeField] GameObject coinPrefab; 
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] float startDelay, interval;
    [SerializeField] int count;
    bool startedSpawning;
    public float myChangedSpeed;
    

    private void Update()
    {
        if (!CustomNetworkManager.Instance.canMove) return;

        if (!startedSpawning)
        {
            StartCoroutine(CoinTimer(60));
            InvokeRepeating(nameof(SpawnObstacle), startDelay, interval);
            startedSpawning = true;
        }
    }

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
    
    public IEnumerator CoinTimer(int seconds)
    {
        count = seconds;

        while (count > 0)
        {
            myChangedSpeed += 1;
            yield return new WaitForSeconds(1);
            count--;
        }
    }
}
