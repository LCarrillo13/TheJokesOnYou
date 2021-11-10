using UnityEngine;

public class WinCheck : MonoBehaviour
{
    [SerializeField] GameManager manager;

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Player")) manager.EndGame();
    }
}
