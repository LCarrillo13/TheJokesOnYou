using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Networking
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] GameObject lobbyCanvas;
        [SerializeField] Button startButton, readyButton, leaveButton;
        [SerializeField] TMP_Dropdown modeDropdown, mapDropdown;

        // only the host can interact with certain GUI elements
        void Awake()
        {
            startButton.interactable = CustomNetworkManager.Instance.IsHost;
            modeDropdown.interactable = CustomNetworkManager.Instance.IsHost;
            mapDropdown.interactable = CustomNetworkManager.Instance.IsHost;
        }

        public void OnClickStartMatch()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.StartMatch();
            lobbyCanvas.SetActive(false);
        }

        // changes the game mode
        public void ChangeMode(int index) => MatchManager.instance.mode = (MatchManager.Mode)index;

        // changes the map 
        public void ChangeMap(int index) => MatchManager.instance.map = (MatchManager.Map)index;
    } 
}
