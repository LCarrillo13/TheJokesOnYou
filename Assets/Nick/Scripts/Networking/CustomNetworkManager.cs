using JetBrains.Annotations;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking
{
	public class CustomNetworkManager : NetworkManager
	{
		public bool canMove;
		[SerializeField] GameObject matchManager;
		Countdown countdown;
		private TimeTrialTimer timeTrialTimer = null;

		// A reference to the CustomNetworkManager version of the singleton. 
		public static CustomNetworkManager Instance => singleton as CustomNetworkManager;

		// attempts to find a player using the passed NetID, this can return null.
		[CanBeNull]
		public static NetworkPlayer FindPlayer(uint _id)
		{
			Instance.players.TryGetValue(_id, out NetworkPlayer player);
			return player;
		}

		/// <summary> Adds a player to the dictionary. </summary>
		public static void AddPlayer([NotNull] NetworkPlayer _player) => Instance.players.Add(_player.netId, _player);

		/// <summary> Removes a player from the dictionary. </summary>
		public static void RemovePlayer([NotNull] NetworkPlayer _player) => Instance.players.Remove(_player.netId);

		/// <summary> A reference to the localplayer of the game. </summary>
		public static NetworkPlayer LocalPlayer
		{
			get
			{
				// If the internal localPlayer instance is null
				if(localPlayer == null)
				{
					// Loop through each player in the game and check if it is a local player
					foreach(NetworkPlayer networkPlayer in Instance.players.Values)
					{
						if(networkPlayer.isLocalPlayer)
						{
							// Set localPlayer to this player as it is the localPlayer
							localPlayer = networkPlayer;
							break;
						}
					}
				}
				// Return the cached local player
				return localPlayer;
			}
		}

		// internal reference to the localPlayer.
		static NetworkPlayer localPlayer;

		//  whether or not this NetworkManager is the host.
		public bool IsHost { get; private set; }      

        public CustomNetworkDiscovery discovery;

		// dictionary of all connected players using their NetID as the key.
		readonly Dictionary<uint, NetworkPlayer> players = new Dictionary<uint, NetworkPlayer>();

		// called when host is started.
		public override void OnStartHost()
		{
			IsHost = true;
			discovery.AdvertiseServer();
		}

        // called when host is stopped.
        public override void OnStopHost() => IsHost = false;

		// called after ServerChangeScene() is run
        public override void OnServerSceneChanged(string sceneName)
        {
			if (sceneName.StartsWith("mode") && sceneName != "mode_Results")
            {
				countdown = FindObjectOfType<Countdown>();

				MatchManager.instance.ChooseMap();
				StartCoroutine(countdown.CountingDown(3));
				if(sceneName == "mode_TimeTrial")
				{
					timeTrialTimer = FindObjectOfType<TimeTrialTimer>();
					StartCoroutine(timeTrialTimer.CountingDownTimer(60));
				}
            }
            base.OnServerSceneChanged(sceneName);
        }
    }
}