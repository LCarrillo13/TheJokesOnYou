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
        [SyncVar] public string winnerName;

        void Awake() => returnToLobbyButton.interactable = CustomNetworkManager.Instance.IsHost;

        void Start()
        {
            winnerName = WinConditions.winner;
            CustomNetworkManager.Instance.canMove = false;
        }

        public void OnClickReturnToLobby()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.ReturnToLobby();
        }

        public void CmdUpdateWinner() => winnerText.text = winnerName;
    } 

}
