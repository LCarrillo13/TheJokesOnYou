using UnityEngine;

public class ReturnToRoom : MonoBehaviour
{
    NetworkManagerLobby networkManager;

    void Awake() => networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();

    public void Return()
    {
        networkManager.ServerChangeScene("Menu");      
    }
}
