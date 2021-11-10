using UnityEngine;

public class WinCheck : MonoBehaviour
{
    NetworkManagerLobby networkManager;

    void Awake() => networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) networkManager.ServerChangeScene("Results");
    }

    
}
