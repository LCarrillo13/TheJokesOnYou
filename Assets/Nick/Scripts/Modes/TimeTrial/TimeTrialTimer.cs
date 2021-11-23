using Mirror;

using Networking;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class TimeTrialTimer : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI myTimerText;
   
    [SyncVar(hook = nameof(OnMyCountdownChanged))] public int count;
    public int delayCount = 3;
    private NetworkScoreboard scoreboard;

    public bool isTimerDone;


    private void Awake()
    {
        if(scoreboard == null)
        {
            scoreboard = FindObjectOfType<NetworkScoreboard>();
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
        }
        else
        {
            count = _new;
            myTimerText.text = count.ToString();
        }
    }

   
} 

