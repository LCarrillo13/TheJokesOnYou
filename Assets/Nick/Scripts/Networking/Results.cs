using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Networking
{
    public class Results : NetworkBehaviour
    {
        [SerializeField] Button returnToLobbyButton;
        public TextMeshProUGUI winnerText;       

        void Awake() => returnToLobbyButton.interactable = CustomNetworkManager.Instance.IsHost;

        public void OnClickReturnToLobby()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.ReturnToLobby();
        }      
    } 
}
