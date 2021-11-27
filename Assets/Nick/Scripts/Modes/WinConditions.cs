using UnityEngine;
using Mirror;

using System;
using System.Linq;

using UnityEngine.SceneManagement;

namespace Networking
{
    public class WinConditions : MonoBehaviour
    {
        public static string winner;
        public int players;
        Scene currentScene;
        public TimeTrialTimer timeTrialTimer;

        void Start() => currentScene = SceneManager.GetActiveScene();

        private void Awake()
        {
            timeTrialTimer = FindObjectOfType<TimeTrialTimer>();
        }

        void Update()
        {
            if(currentScene.name == "mode_TimeTrial")
            {
                TimeTrialWinner();
                
            }
            else if (currentScene.name != "mode_Survival") return;
            CheckForLastPlayerLeft();
        }

        void CheckForLastPlayerLeft()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            players = playerObjects.Length;

            if (players == 1)
            {
                foreach (GameObject player in playerObjects)
                {
                    if (player.gameObject.activeInHierarchy)
                    {
                        winner = player.GetComponentInChildren<TextMesh>().text;
                        CustomNetworkManager.Instance.ServerChangeScene("mode_Results");
                        
                    }
                }
            }
        }

        void TimeTrialWinner()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            players = playerObjects.Length;
            if (players == 1)
            {
                foreach (GameObject player in playerObjects)
                {
                    if (player.gameObject.activeInHierarchy)
                    {
                        winner = player.GetComponentInChildren<TextMesh>().text;
                        CustomNetworkManager.Instance.ServerChangeScene("mode_Results");
                        
                    }
                }
            }
            else
            {
                winner = "HighScore: " + timeTrialTimer.winnerString;
            }
            
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                winner = collision.collider.GetComponentInChildren<TextMesh>().text;
                CustomNetworkManager.Instance.ServerChangeScene("mode_Results");
            }
        }
    } 
}
