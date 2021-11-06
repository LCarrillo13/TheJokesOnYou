using Mirror;

using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] float speed;

    void Update() => Move();

    void Move() => transform.Translate(Vector3.down * speed * Time.deltaTime);

    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Player"))
        {
            NetworkServer.Destroy(other.gameObject.GetComponent<PlayerMovement>().tempCamera);
            NetworkServer.Destroy(other.gameObject);
        }
    }

}
