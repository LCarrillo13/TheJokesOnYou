using UnityEngine;

public class Host : MonoBehaviour
{
    [SerializeField] NetworkManagerLobby networkManager = null;
    [SerializeField] GameObject lobbyPanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();
        lobbyPanel.SetActive(false);
    }
}

