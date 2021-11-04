using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject playerObj;


    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit by player");

        }
    }
}
