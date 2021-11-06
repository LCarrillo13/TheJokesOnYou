using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-room-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkRoomManager.html

	See Also: NetworkManager
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

// This is a specialized NetworkManager that includes a networked room.
// The room has slots that track the joined players, and a maximum player count that is enforced.
// It requires that the NetworkRoomPlayer component be on the room player objects.
// NetworkRoomManager is derived from NetworkManager, and so it implements many of the virtual functions provided by the NetworkManager class.
public class CustomNetworkRoomManager : NetworkRoomManager
{
    [SerializeField] GameManager manager;
    [Scene] public string raceScene, survivalScene, thirdScene;

    public override void Awake() => manager = GameObject.Find("Manager - Game").GetComponent<GameManager>();

    #region Server Callbacks
    // This is called on the server when the server is started - including when a host is started.
    public override void OnRoomStartServer() 
    { 
        
    }

    // This is called on the server when the server is stopped - including when a host is stopped.
    public override void OnRoomStopServer() { }

    // This is called on the host when a host is started.
    public override void OnRoomStartHost() { }

    // This is called on the host when the host is stopped.
    public override void OnRoomStopHost() { }

    // This is called on the server when a new client connects to the server.
    // conn - The new connection.
    public override void OnRoomServerConnect(NetworkConnection conn) { }

    // This is called on the server when a client disconnects.
    // conn - The connection that disconnected
    public override void OnRoomServerDisconnect(NetworkConnection conn) { }

    // This is called on the server when a networked scene finishes loading.
    // sceneName - Name of the new scene.
    public override void OnRoomServerSceneChanged(string sceneName) 
    {
        manager = GameObject.Find("Manager - Game").GetComponent<GameManager>();
    }

    // This allows customization of the creation of the room-player object on the server.
    // By default the roomPlayerPrefab is used to create the room-player, but this function allows that behaviour to be customized.
    // conn - The connection the player object is for
    // returns - The new room-player object.
    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        return base.OnRoomServerCreateRoomPlayer(conn);
    }

    // This allows customization of the creation of the GamePlayer object on the server.
    // By default the gamePlayerPrefab is used to create the game-player, but this function allows that behaviour to be customized. The object returned from the function will be used to replace the room-player on the connection.
    // conn - The connection the player object is for.
    // roomPlayer - The room player object for this connection.
    // returns - A new GamePlayer object.
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        return base.OnRoomServerCreateGamePlayer(conn, roomPlayer);
    }

    // This allows customization of the creation of the GamePlayer object on the server.
    // This is only called for subsequent GamePlay scenes after the first one.
    // See OnRoomServerCreateGamePlayer to customize the player object for the initial GamePlay scene.
    // conn - The connection the player object is for.
    public override void OnRoomServerAddPlayer(NetworkConnection conn)
    {
        base.OnRoomServerAddPlayer(conn);
    }

    // This is called on the server when it is told that a client has finished switching from the room scene to a game player scene.
    // When switching from the room, the room-player is replaced with a game-player object. This callback function gives an opportunity to apply state from the room-player to the game-player object.
    // conn - The connection of the player
    // roomPlayer - The room player object.
    // gamePlayer - The game player object.
    // returns - False to not allow this player to replace the room player.
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
    }

    // This is called on the server when all the players in the room are ready.
    // The default implementation of this function uses ServerChangeScene() to switch to the game player scene. By implementing this callback you can customize what happens when all the players in the room are ready, such as adding a countdown or a confirmation for a group leader.
    public override void OnRoomServerPlayersReady()
    {
        switch (manager.mode)
        {
            case GameManager.Mode.Race:
                ServerChangeScene(raceScene);
                break;
            case GameManager.Mode.Survival:
                ServerChangeScene(survivalScene);
                break;
            default:
                break;
            case GameManager.Mode.Mode3:
                ServerChangeScene(thirdScene);
                break;
        }
    }

    // This is called on the server when CheckReadyToBegin finds that players are not ready
    // May be called multiple times while not ready players are joining
    public override void OnRoomServerPlayersNotReady() { }
    #endregion

    #region Client Callbacks
    // This is a hook to allow custom behaviour when the game client enters the room.
    public override void OnRoomClientEnter() { }

    // This is a hook to allow custom behaviour when the game client exits the room.
    public override void OnRoomClientExit() { }

    // This is called on the client when it connects to server.
    // conn - The connection that connected.
    public override void OnRoomClientConnect(NetworkConnection conn) { }

    // This is called on the client when disconnected from a server.
    // conn - The connection that disconnected.</param>
    public override void OnRoomClientDisconnect(NetworkConnection conn) { }

    // This is called on the client when a client is started.
    // roomClient - The connection for the room.
    public override void OnRoomStartClient() { }

    // This is called on the client when the client stops.
    public override void OnRoomStopClient() { }

    // This is called on the client when the client is finished loading a new networked scene.
    // conn - The connection that finished loading a new networked scene.
    public override void OnRoomClientSceneChanged(NetworkConnection conn) { }

    // Called on the client when adding a player to the room fails.
    // This could be because the room is full, or the connection is not allowed to have more players.
    public override void OnRoomClientAddPlayerFailed() { }
    #endregion

    #region Optional UI
    public override void OnGUI()
    {
        base.OnGUI();
    }
    #endregion
}

