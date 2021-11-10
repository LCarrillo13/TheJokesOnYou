using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [SerializeField] GameObject lobbyUI = null;
    [SerializeField] TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] Image[] playerReadyImages = new Image[4];
    [SerializeField] Button startGameButton = null;
    [SyncVar(hook = nameof(HandleDisplayNameChanged))] public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))] public bool IsReady = false;

    bool isLeader;
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    NetworkManagerLobby room;
    NetworkManagerLobby Room
    {
        get
        {
            if (room != null) return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for non-existent friends...";
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyImages[i].color = Room.RoomPlayers[i].IsReady ? Color.green : Color.red; 
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        // only host can press start button
        if (!isLeader) return;
        startGameButton.interactable = readyToStart;
    }

    [Command]
    void CmdSetDisplayName(string displayName) => DisplayName = displayName;

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return; 
        Room.StartGame();
    }

    [Command]
    public void CmdDisconnect()
    {
        if (isServer)
        {
            room.StopHost(); 
        }
        else
        {
            if (!isLocalPlayer) return;
            room.OnClientDisconnect(connectionToClient);
        }
        room.mainMenuPanel.SetActive(true);
    }
}
