using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Join : MonoBehaviour
{
    [SerializeField] NetworkManagerLobby networkManager = null;
    [SerializeField] GameObject lobbyPanel = null;
    [SerializeField] TMP_InputField ipAddressInputField = null;
    [SerializeField] Button joinButton = null;

    void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();
        joinButton.interactable = false;
    }

    void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    void HandleClientDisconnected() => joinButton.interactable = true;
}
