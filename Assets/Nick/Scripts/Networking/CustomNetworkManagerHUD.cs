using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CustomNetworkManagerHUD : NetworkManagerHUD
{
    NetworkManager networkManager;
    [SerializeField] Button hostButton;
    [SerializeField] Button connectButton;

    void Awake() => networkManager = GetComponent<NetworkManager>();

    void Start()
    {
        hostButton.onClick.AddListener(networkManager.StartHost);
        connectButton.onClick.AddListener(networkManager.StartClient);
    }
}
