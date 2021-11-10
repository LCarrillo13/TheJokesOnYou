using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;

public class NetworkManagerLobby : NetworkManager
{
    #region Variables
    [SerializeField] int minPlayers = 2;
    [SerializeField] string menuScene = string.Empty;
    [SerializeField] string map_Race = string.Empty, map_Survival = string.Empty, map_TimeTrial = string.Empty;
    [Header("Maps")]
    //[SerializeField] int numberOfRounds = 1;
    //[SerializeField] MapSet mapSet = null;
    [SerializeField] GameObject map1, map2;
    [Header("Room")]
    [SerializeField] NetworkRoomPlayerLobby roomPlayerPrefab = null;
    [Header("Game")]
    [SerializeField] NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] GameObject playerSpawnSystem = null;
    [Header("Menu")]
    GameObject mainMenuPanel;
    //[SerializeField] GameObject roundSystem = null;
    //MapHandler mapHandler;
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;
    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();
    #endregion

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    // returns true if all players have pressed 'Ready' button
    bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) return false;
        }
        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart()) return;

            //mapHandler = new MapHandler(mapSet, numberOfRounds);
            //ServerChangeScene(mapHandler.NextMap);

            UpdateMode();
        }
    }

    void UpdateMode()
    {
        switch (GameManager.mode)
        {
            case GameManager.Mode.Race:
                ServerChangeScene(map_Race);
                break;
            case GameManager.Mode.Survival:
                ServerChangeScene(map_Survival);
                break;
            case GameManager.Mode.TimeTrial:
                ServerChangeScene(map_TimeTrial);
                break;
            default:
                break;
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // from menu to game
        // '.StartsWith' - determines whether the beginning of this string instance matches the specified string.
        if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("map"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            //GameObject roundSystemInstance = Instantiate(roundSystem);
            //NetworkServer.Spawn(roundSystemInstance);

            UpdateMap();
        }

        if (sceneName == "Menu" || sceneName == "Results") ShowCursor();

        // if returning from game scene to menu and server is still active
        // this replaces the gameplayers with room players so you can return to lobby from a game scene and start again
        if (sceneName == "Menu" && isNetworkActive)
        {
            ReplaceConnections();
            UpdateUI();
        }
    }

    void ReplaceConnections()
    {
        for (int i = GamePlayers.Count - 1; i >= 0; i--)
        {
            var conn = GamePlayers[i].connectionToClient;
            var roomPlayerInstance = Instantiate(roomPlayerPrefab);
            NetworkServer.Destroy(conn.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    void UpdateUI()
    {
        mainMenuPanel = GameObject.Find("Panel - Main Menu");
        mainMenuPanel.SetActive(false);
    }

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateMap()
    {
        switch (GameManager.map)
        {
            case GameManager.Map.Day:
                GameObject map1Instance = Instantiate(map1);
                NetworkServer.Spawn(map1Instance);
                break;
            case GameManager.Map.Night:
                GameObject map2Instance = Instantiate(map2);
                NetworkServer.Spawn(map2Instance);
                break;
            default:
                break;
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }
}
