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

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}