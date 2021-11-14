using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Networking
{
    public class Lobby : MonoBehaviour
    {
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
