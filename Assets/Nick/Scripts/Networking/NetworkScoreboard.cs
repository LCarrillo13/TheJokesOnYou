// Creator: Lucas C
// Creation Time: 2021/11/23 2:46 PM
using Mirror;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Networking
{
	public class NetworkScoreboard : NetworkBehaviour
	{
		[SerializeField] private SyncDictionary<uint, int> scores = new SyncDictionary<uint, int>();

		public void SetScore(uint _netid) => CmdSetScore(_netid);

		private void Start()
		{
			scores.Callback += OnRecieveScores;
		}

		private void OnRecieveScores(SyncIDictionary<uint, int>.Operation _op, uint _key, int _item)
		{
			// Do things with the scoreboard to render them here
			
			// TODO: Write Sorting algo here to sort player scores highest to lowest,
			// TODO: set highest score as winner
			// BUG help
			// Parse Score int to String, then set Canvas UI Text = score string
			// Do same thing with player names

			foreach(KeyValuePair<uint, int> pair in scores)
			{
				NetworkPlayer player = CustomNetworkManager.FindPlayer(pair.Key);
				
				// Do things with this score here
				// 
			}
		}
		public void AssignPlayer(uint _netId) => CmdAssignPlayer(_netId);
		[Command]
		private void CmdAssignPlayer(uint _netId)
		{
			scores.Add(_netId, 0);
		}
		[Command]
		private void CmdSetScore(uint _netId)
		{
			scores[_netId] = 5; //or whatever the score should be
		}
	}
}