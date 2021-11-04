using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    
    // [SerializeField] private GameObject spawnPt1;
    // [SerializeField] private GameObject spawnPt2;
    // [SerializeField] private GameObject spawnPt3;
    
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;
        
    [SerializeField] private float interval = 3f;
    [SerializeField] private float delay = 5f;


    // too complicated to try random spawn points rn
    //[SerializeField] private Transform[] spawnPoints = new Transform[3];
   
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), delay, interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnObstacle()
    {
        Instantiate(obstaclePrefab, point1);
    }
}
