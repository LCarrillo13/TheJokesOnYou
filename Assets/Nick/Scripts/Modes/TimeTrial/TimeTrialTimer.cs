using Mirror;

using Networking;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTrialTimer : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI myTimerText;
   
    [SyncVar(hook = nameof(OnMyCountdownChanged))] public int count;
    public int delayCount = 3;
    public int players;
    //public int scores;
    //private NetworkScoreboard scoreboard;
    public ScoreSystem scoreSystem;
    public WinConditions winConditions;

    public bool isTimerDone;
    public bool isCorDone = false;
    public string winnerString;
    public int winnerInt;
    public List<int> pScores = new List<int>();
    public List<string> pNames = new List<string>();
    Scene currentScene;


    private void Awake()
    {
        if(currentScene.name != "mode_TimeTrial")
        {
            this.enabled = false;
        }
        winConditions = FindObjectOfType<WinConditions>();
        if(scoreSystem == null)
        {
            scoreSystem = gameObject.transform.GetComponent<ScoreSystem>();
        }
    }

    private void Update()
    {
        if(currentScene.name != "mode_TimeTrial")
        {
            this.enabled = false;
        }
    }

    [Server]
    public IEnumerator CountingDownTimer(int _seconds)
    {
        count = _seconds;
        while(delayCount > 0)
        {
            yield return new WaitForSeconds(1);
            delayCount--;
        }

        while (count > 0)
        {       
            yield return new WaitForSeconds(1);
            count--;
        }
        
        yield return new WaitForSeconds(1);
        isTimerDone = true;

        // End Game stuff here 
    }

    public void OnMyCountdownChanged(int _old, int _new)
    {
        if (count == 0)
        {
            myTimerText.text = "Finish!";
            myTimerText.CrossFadeAlpha(0, 2, false);
            
            CheckPlayerScores();

        }
        else
        {
            count = _new;
            myTimerText.text = count.ToString();
        }
    }

    void CheckPlayerScores()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = playerObjects.Length;
        int[] scores = new int[playerObjects.Length];
        // int tempInt = 0;
        // int temp = 0;
        // stuff\
        //BUG not sure if List<> even works might need Networked List which is a pain
        
        //BUG Update: fuck this doesnt work either... i hate networks
        
        //meh names dont matter, just display the highest score at end
        
        foreach(GameObject player in playerObjects)
        {
            if(player.gameObject.activeInHierarchy)
            {
                pScores.Add(player.GetComponent<ScoreSystem>().savedScore);
                //pNames.Add(pScores[playerObjects.Length].ToString());
                pNames.Add(player.GetComponentInChildren<TextMesh>().text);
            }
            
            // BUG so yeah Array doesnt work, too complicated, just use List<>s
            
            //scores[tempInt] = player.GetComponent<ScoreSystem>().savedScore;
            // if(scores[tempInt] > scores[tempInt + 1])
            // {
            //     temp = scores[tempInt + 1];
            //     scores[tempInt + 1] = scores[tempInt];
            //     scores[tempInt] = temp;
            //
            // }
            //tempInt++;
        }

        winnerInt = pScores.Max();
        winnerString = winnerInt.ToString();
        
        
        //pScores.Sort();
        //pScores.Last();

        //winnerInt = scores.GetUpperBound(Int32.MaxValue);
        //scores.Append(scoreSystem.savedScore);
           
            //     CustomNetworkManager.Instance.ServerChangeScene("mode_Results");

            // BUG this doesnt work cuz some dumb reason idk...
        // for(int i = 0; i <= playerObjects.Length; i++)
        // {
        //     for(int j = 0; j <= playerObjects.Length - 1; j++)
        //     {
        //         if(j > j + 1)
        //         {
        //             // temp stuff
        //              temp = scores[tempInt + 1];
        //              scores[tempInt + 1] = scores[tempInt];
        //              scores[tempInt] = temp;
        //         }
        //     }
        //     
        // }
        
        if(isCorDone == false)
        {
            StartCoroutine(FinishedTime());
        }
    }

    public IEnumerator FinishedTime()
    {
        yield return new WaitForSeconds(1);
        CustomNetworkManager.Instance.ServerChangeScene("mode_Results");
        isCorDone = true;

    }
   
} 

