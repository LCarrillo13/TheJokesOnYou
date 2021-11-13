using UnityEngine;
using Networking;

public class WinCheck : MonoBehaviour
{
    CustomNetworkManager networkManager;

    void Awake() => networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) networkManager.ServerChangeScene("Results");
    }    
}
