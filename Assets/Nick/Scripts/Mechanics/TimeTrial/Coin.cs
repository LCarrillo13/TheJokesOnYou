using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


using System;

using Unity.Collections;

public class Coin : NetworkBehaviour
{
	[SerializeField] private GameObject thisCoin;
	[SerializeField] float coinSpeed;
	[SyncVar] ScoreSystem myScore;

	private void OnEnable()
	{
		thisCoin = GetComponent<GameObject>();
		myScore = myScore;
	}

	void Update() => Move();

	void Move() => transform.Translate(-Vector3.forward * coinSpeed * Time.deltaTime);

	[Server]
	void OnCollisionEnter(Collision other)
	{    
		if(other.collider.CompareTag("Player"))
		{
			//ok so like this works but AddScore doesnt wtf
			AddScore();
			NetworkServer.Destroy(thisCoin);
			Debug.Log("player hit coin");
			
		}
	}
	/// <summary>
	/// Adds +1 to player score
	/// </summary>
	void AddScore()
	{
		//THIS IS NOT WORKING AND I HAVE NO IDEA WHY
		//I TRIED EVERYTHING AND I STILL GET
		//STUPID NULLREFERENCEEXEPTION ERRORS EVERY TIME
		// /////////////////////////
		// myScore.playerScore += 1;
		// ////////////////////////
		Debug.Log("Score added");
	}
}
