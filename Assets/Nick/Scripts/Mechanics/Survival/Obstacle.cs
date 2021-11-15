using Mirror;
using Networking;
using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] float speed;
    WinConditions winConditions;

    void Awake() => winConditions = GameObject.Find("Manager - General").GetComponent<WinConditions>();

    void Update() => Move();

    void Move() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            winConditions.players -= 1;
        }
    }

}
