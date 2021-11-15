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
		//thisCoin = GetComponent<GameObject>().gameObject;
		myScore = myScore;
	}

	void FixedUpdate()
	{
		Move();
		//thisCoin = GetComponent<GameObject>().gameObject;
	}
	

	void Move() => transform.Translate(-Vector3.forward * coinSpeed * Time.deltaTime);

	[Server]
	void OnCollisionEnter(Collision other)
	{    
		if(other.collider.CompareTag("Player"))
		{
			
			other.collider.GetComponent<ScoreSystem>().playerScore += 1;
			
			//NetworkServer.Destroy(thisCoin);
			Debug.Log("player hit coin");
			
		}
	}
	
}
