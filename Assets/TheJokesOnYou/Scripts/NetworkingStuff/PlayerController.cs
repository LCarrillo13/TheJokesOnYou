using System;

using UnityEngine;

namespace NetworkGame
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float speed = 3f;
		[SerializeField] private float sprint = 6f;

		// Update is called once per frame
		private void Update()
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				Sprint();
			}
			else
			{
				Walk();
			}
		}

		private void Walk()
		{
			transform.position += transform.right * Time.deltaTime * speed * Input.GetAxis("Horizontal");
			transform.position += transform.forward * Time.deltaTime * speed * Input.GetAxis("Vertical");
		}
		private void Sprint()
		{
			transform.position += transform.right * Time.deltaTime * sprint * Input.GetAxis("Horizontal");
			transform.position += transform.forward * Time.deltaTime * sprint * Input.GetAxis("Vertical");
		}
	}
}