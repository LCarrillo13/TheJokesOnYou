using Mirror;
using Networking;

using System;

using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] float speed;
    WinConditions winConditions;
    ObstacleSpawner obstacleSpawner;
    public GameObject coinCollider;

    void Awake()
    {
        winConditions = FindObjectOfType<WinConditions>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        
    }

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 9);
    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
       
        Move();
        UpdateSpeed();
    }

    void Move() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void UpdateSpeed() => speed = obstacleSpawner.changedSpeed;

    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            winConditions.players -= 1;
        }
        
        
    
    }

}
