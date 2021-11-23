using Mirror;

using Networking;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NetworkPlayer = Networking.NetworkPlayer;

public class ScoreSystem : NetworkBehaviour
{
    // Local Player Score tracking
    [SyncVar(hook = nameof(OnChangePlayerScore))] [SerializeField] public int playerScore = 0;

    [SerializeField] private Text scoreText;
    
    //Script references
    //private NetworkScoreboard scoreboard;
    //private TimeTrialTimer timeTrialTimer;

    // Leaderboards tracking 
    
    [SerializeField] [SyncVar] private int savedScore;
    private int localPlayerScore;
    private string playerID = String.Empty;
    //public int[] playerScores = new int[GameObject.FindGameObjectsWithTag("Player").Length];
    //public string[] playerNames = new string[GameObject.FindGameObjectsWithTag("Player").Length];
    private void Awake()
    {
        if(isLocalPlayer)
        {
            //playerID = //GetComponentInChildren<TextMesh>().text;
        }
        
    }

    private void Update()
    {
        localPlayerScore = savedScore;
        // Assign script references
        
        // if(scoreboard == null)
        // {
        //     scoreboard = FindObjectOfType<NetworkScoreboard>();
        // }
        // if(timeTrialTimer == null)
        // {
        //     timeTrialTimer = FindObjectOfType<TimeTrialTimer>();
        // }
        
        // check if game is over
       // if(timeTrialTimer.isTimerDone)
        //{
            //scoreboard.SetScore(netId);
        //}
    }

    /// <summary>
    /// Updates score text and saved score temp variable for local player
    /// </summary>
    /// <param name="_old">N/A</param>
    /// <param name="_new">N/A</param>
    void OnChangePlayerScore(int _old, int _new)
    {
        scoreText.text = playerScore.ToString();
        if(isLocalPlayer)
        {
            savedScore = playerScore;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //scoreText.text = playerScore.ToString();
        localPlayerScore = savedScore;
    }
    
    //[ServerCallback]
    
    
}
