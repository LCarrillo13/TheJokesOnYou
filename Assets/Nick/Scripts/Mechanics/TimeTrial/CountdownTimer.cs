using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CountdownTimer : NetworkBehaviour
{
    [SerializeField] private int startTime;
    [SerializeField] private float gameTimerCount;
    [SerializeField] private double currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = NetworkTime.time;
    }

    // Update is called once per frame
    void Update()
    {

        if(isClient)
        {
            // Coroutine here
        }
            
        
    }

    IEnumerator Countdown()
    {
        float duration = 3f; // 3 seconds you can change this 
        //to whatever you want
        float normalizedTime = 0;
        while(normalizedTime <= 1f)
        {
            //countdownImage.fillAmount = normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        // Timer code here
        
    }
}
