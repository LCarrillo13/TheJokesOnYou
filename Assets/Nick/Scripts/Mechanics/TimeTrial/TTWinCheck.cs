using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Networking;

using System;
using System.Linq;

using NetworkPlayer = Networking.NetworkPlayer;

public class TTWinCheck : NetworkBehaviour
{
    public static ScoreSystem scoreSys;
    public static NetworkPlayer[] numPlayers = FindObjectsOfType<NetworkPlayer>();
    
    [SerializeField] private int activePlayerCount;
    [SyncVar]private int finalScore = scoreSys.playerScore;
    public static string winner = String.Empty;
    public NetworkManager netManager;
    [SyncVar] public bool isGameOver = false;
    public int indexCount = numPlayers.Length;
    [Header("UI Elements")] [SerializeField] private Text finalScoreText;
    [SerializeField] private Text[] playerScores;


    private void Awake()
    {
        // Debug.Log(activePlayerCount);
        // activePlayerCount = netManager.numPlayers;
        // Debug.Log(activePlayerCount);
        // Debug.Log(netManager.numPlayers);
        //public int indexCount = numPlayers.Length;
        
}

    private void FixedUpdate()
    {
        if(isGameOver)
        {
            EndGame();
        }
    }
    void EndGame()
    {
        //numPlayers = new NetworkPlayer[activePlayerCount];
        foreach(NetworkPlayer player in numPlayers)
        {
            if(player.gameObject.activeInHierarchy)
            {
                
            }
        }
        if(activePlayerCount > 1) return;
        foreach(NetworkPlayer player in numPlayers)
        {
            if(player.gameObject.activeInHierarchy)
            {
                //GetComponent<ScoreSystem>().playerScore
                winner = player.GetComponentInChildren<TextMesh>().text;
                // Update Scoreboard text to add player name and score
                finalScoreText.text = winner;
            }
        }
        
        netManager.ServerChangeScene("mode_Results");
        // Ends the game

    }
}
