using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

using Networking;
using static TTWinCheck;

using NetworkPlayer = Networking.NetworkPlayer;

public class CountdownTimer : NetworkBehaviour
{
    [SerializeField] private int countdownTime;
    [SerializeField] public Text countdownDisplay;
    [SerializeField] private int maxTime = 60;
    [SerializeField] public CustomNetworkManager nManager;
    [SerializeField] public TTWinCheck ttWinCheck;
   // private readonly NetworkPlayer[] numPlayers = FindObjectsOfType<NetworkPlayer>();
    
    /// <summary>
    /// Countdown Timer that limits game time
    /// </summary>
    /// <returns>countdownTime</returns>
    IEnumerator Countdown()
     {
         while(countdownTime > maxTime)
         {
             countdownDisplay.text = countdownTime.ToString();
             yield return new WaitForSeconds(1f);

             countdownTime--;
             
         }

         countdownDisplay.text = "End!"; 
         
         // EndGame();
         ttWinCheck.isGameOver = true;
     }

    
    //[Server]
    // void EndGame1()
    // {
    //     foreach(NetworkPlayer player in numPlayers)
    //     {
    //         if(player.gameObject.activeInHierarchy)
    //         {
    //           // Update and Display player score on Results
    //           
    //         }
    //         else
    //         {
    //             // Set player score to 0 and Display
    //         }
    //     }
    //     // Ends the game
    //     
    // }
}
