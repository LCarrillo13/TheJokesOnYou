using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Networking
{
    public class Results : NetworkBehaviour
    {
        [SerializeField] Button returnToLobbyButton;
        [SerializeField] TextMeshProUGUI winnerText;
        [SyncVar(hook = nameof(OnTextChanged))] public string winnerString;

        void Awake() => returnToLobbyButton.interactable = CustomNetworkManager.Instance.IsHost;

        void Start() => winnerText.text = WinCheck.winner + " is the pro gamer!";

        public void OnClickReturnToLobby()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.ReturnToLobby();
        }

        public void OnTextChanged(string _old, string _new)
        {

        }
    } 
}
