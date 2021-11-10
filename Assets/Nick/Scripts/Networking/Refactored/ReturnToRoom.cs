using UnityEngine;
using Mirror;

public class ReturnToRoom : NetworkBehaviour
{
    NetworkManagerLobby networkManager;

    void Awake() => networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();

    public void Return()
    {
        if (!isServer) return;
        networkManager.ServerChangeScene("Menu");      
    }
}
