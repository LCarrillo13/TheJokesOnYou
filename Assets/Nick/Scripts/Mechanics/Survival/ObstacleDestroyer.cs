using Mirror;

using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Coin") )
        {
            NetworkServer.Destroy(other.gameObject);
        }
    }
}
