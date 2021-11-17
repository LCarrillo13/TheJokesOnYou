using Mirror;
using Networking;
using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] float speed;
    WinConditions winConditions;
    ObstacleSpawner obstacleSpawner;

    void Awake()
    {
        winConditions = FindObjectOfType<WinConditions>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
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
