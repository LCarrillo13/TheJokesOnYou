using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] GameObject playerPrefab;

        // called if we are the local player and NOT a remote player
        public override void OnStartLocalPlayer()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
        }

        // called similarly to Start() for client and host
        public override void OnStartClient() => CustomNetworkManager.AddPlayer(this);

        // called when client or host disconnects
        public override void OnStopClient() => CustomNetworkManager.RemovePlayer(this);

        #region Lobby 

        #region Starting Game
        public void StartMatch()
        {
            if (!isLocalPlayer) return;
            CmdStartMatch();
        }

        [Command]
        public void CmdStartMatch() => MatchManager.instance.StartMatch();
        #endregion

        #region Disconnecting
        public void LeaveMatch()
        {
            if (!isLocalPlayer) return;
            CmdLeaveMatch();
        }

        [Client]
        public void CmdLeaveMatch()
        {
            CustomNetworkManager.Instance.StopClient();
            if (isServer) CustomNetworkManager.Instance.StopHost();
            SceneManager.LoadScene("Menu");
        }
        #endregion

        #region Readying
        public void Ready()
        {
            if (!isLocalPlayer) return;
            CmdReady();
        }

        [Command]
        public void CmdReady()
        {

        }
        #endregion

        #endregion
    }
}