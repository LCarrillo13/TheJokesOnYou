using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class CustomNetworkRoomUI : MonoBehaviour
    {
        [SerializeField] CustomNetworkRoomManager networkManager;
        [SerializeField] Button startButton;

        private void Awake() => networkManager = GameObject.Find("NetworkRoomManager").GetComponent<CustomNetworkRoomManager>();

        void Start() => startButton.onClick.AddListener(OnClickStart);

        void OnClickStart() => networkManager.OnRoomServerPlayersReady();
    }
}