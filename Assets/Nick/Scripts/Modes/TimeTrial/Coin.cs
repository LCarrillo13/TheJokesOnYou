using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


using System;

using Unity.Collections;

using UnityEngine.SceneManagement;

public class Coin : NetworkBehaviour
{
	[SerializeField] private GameObject thisCoin;
	[SerializeField] float coinSpeed;
	[SyncVar] ScoreSystem myScore;
	[SerializeField]public GameObject obstacleCollider;
	[SerializeField] public GameObject scorePanel;
	private CoinSpawner coinSpawner;
	private bool isTT;

	private void OnEnable()
	{
		
		//thisCoin = GetComponent<GameObject>().gameObject;
		myScore = myScore;
		
	}

	private void Awake()
	{
		coinSpawner = FindObjectOfType<CoinSpawner>();
	}

	void FixedUpdate()
	{
		Move();
		UpdateCoinSpeed();
		//thisCoin = GetComponent<GameObject>().gameObject;
	}

	void UpdateCoinSpeed()
	{
		coinSpeed = coinSpawner.myChangedSpeed;
	}
	

	void Move() => transform.Translate(Vector3.down * coinSpeed * Time.deltaTime);

	[Server]
	void OnCollisionEnter(Collision other)
	{    
		if(other.collider.CompareTag("Player"))
		{
			FindObjectOfType<ScoreSystem>().playerScore += 1;
			//scorePanel.GetComponent<ScoreSystem>().playerScore += 1;
			
			NetworkServer.Destroy(gameObject);
			Debug.Log("player hit coin");
			
		}
	}
	
}
