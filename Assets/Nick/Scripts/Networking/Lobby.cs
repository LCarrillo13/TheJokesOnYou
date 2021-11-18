using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Networking
{
    public class Lobby : NetworkBehaviour
    {
        [SerializeField] Button startButton, readyButton, leaveButton;
        [SerializeField] TMP_Dropdown modeDropdown, mapDropdown;

        // only the host can interact with certain GUI elements
        void Awake()
        {
            startButton.gameObject.SetActive(CustomNetworkManager.Instance.IsHost);
            modeDropdown.gameObject.SetActive(CustomNetworkManager.Instance.IsHost);
            mapDropdown.gameObject.SetActive(CustomNetworkManager.Instance.IsHost);
        }

        public void OnClickStartMatch()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.StartMatch();
        }

        public void OnClickLeaveMatch()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.LeaveMatch();
        }

        public void OnClickReady()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.Ready();
        }

        // changes the game mode
        public void ChangeMode(int index) => MatchManager.instance.mode = (MatchManager.Mode)index;

        // changes the map 
        public void ChangeMap(int index) => MatchManager.map = (MatchManager.Map)index;
    } 
}
