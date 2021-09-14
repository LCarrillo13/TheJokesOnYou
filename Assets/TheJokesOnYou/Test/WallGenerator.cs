using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheJokesOnYou.Mechanics
{
    public class WallGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab1;
        [SerializeField] private GameObject wallPrefab2;
        [SerializeField] private GameObject wallPrefab3;
        [SerializeField] private GameObject wallPrefab4;
        [SerializeField] private int randomNum = 0;
        [SerializeField] private Vector3 targetSpawn;
        [SerializeField] private Vector3 targetOffset = Vector3.forward;
        [SerializeField] private Vector3 quartTest = Vector3.one;
        [SerializeField] private Quaternion spawnQuaternion;
        [SerializeField] private Transform targetTransform;

        void GenerateWall()
        {
            Instantiate(wallPrefab1, targetSpawn, spawnQuaternion);
        }
        // Start is called before the first frame update
        void Start()
        {
            targetSpawn = transform.position + targetOffset;
            spawnQuaternion = targetTransform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown("space"))
            {
                GenerateWall();
            }
        }
    }
}