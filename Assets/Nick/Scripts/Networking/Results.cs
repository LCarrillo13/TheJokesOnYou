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
        [SyncVar(hook = nameof(OnWinnerFound))] public string winnerName;

        // only host can interact with this button
        void Awake() => returnToLobbyButton.interactable = CustomNetworkManager.Instance.IsHost;

        void Start()
        {
            FindWinner();
            CustomNetworkManager.Instance.canMove = false;
        }

        // when "winnerName" is changed below, this changes it for all clients
        public void OnWinnerFound(string _old, string _new)
        {
            winnerName = _new;
            winnerText.text = winnerName + " is the pro gamer!";
        }

        // server finds winner
        [Server]
        public void FindWinner() => winnerName = WinConditions.winner;

        // returns all players to the lobby scene
        public void OnClickReturnToLobby()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.ReturnToLobby();
        }
    } 

}
