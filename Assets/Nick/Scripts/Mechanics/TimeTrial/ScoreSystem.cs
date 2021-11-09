using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : NetworkBehaviour
{
    [SyncVar] [SerializeField] public int playerScore = 0;

    [SerializeField] private Text scoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        scoreText.text = playerScore.ToString();
    }
    
    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Coin"))
        {
            NetworkServer.Destroy(other.gameObject);
        }
    }
}
