using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;

    void Update() => Move();

    void Move() => transform.Translate(Vector3.down * speed * Time.deltaTime);

    void OnCollisionEnter(Collision other)
    {    
        if(other.collider.CompareTag("Player"))
        {
            print(other.gameObject.name + " has died!");
        }
    }

}
