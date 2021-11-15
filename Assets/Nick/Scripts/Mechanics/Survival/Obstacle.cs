using Mirror;

using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] float speed;

    void Update() => Move();

    void Move() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
