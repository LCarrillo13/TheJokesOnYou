using System;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    CharacterController controller;
    [SerializeField] float idleSpeed;
    [SerializeField] float slowClimbSpeed;
    [SerializeField] float fastClimbSpeed;
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] LayerMask obstacleLayer;
    private String layerString = "Obstacle";
    [SerializeField] Vector3 raycastOffset;
    [SerializeField] bool hasCollided = false;
    float moveSpeed;
    Vector3 direction;
    RaycastHit hit;

    void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
        Debug.Log(layerString);
    }

    void Update()
    {
        if (!player.canMove) return; // Guard Clause prevents unnecessary update cycles until the if statement returns false

        if (Climbable() && !hasCollided)
        {
            Move(); 
        }
        else
        {
            Fall();
        }
        //Redundant
        // if(hasCollided)
        // {
        //     Fall();
        // }
    }

    void Move()
    {
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        direction = new Vector3(moveX, moveY, 0);
        direction = transform.TransformDirection(direction);

        if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastClimbSpeed;
            player.playerAnimation.FastClimbAnimation();
        }
        else if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = slowClimbSpeed;
            player.playerAnimation.SlowClimbAnimation();
        }
        else if (direction == Vector3.zero)
        {
            moveSpeed = idleSpeed;
            player.playerAnimation.IdleAnimation();
        }

        direction *= moveSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    bool Climbable()
    {
        Debug.DrawRay(transform.position + raycastOffset, Vector3.forward, Color.red, 2);
        if (Physics.Raycast(transform.position + raycastOffset, Vector3.forward, out hit, 2, climbableLayer))
        {
            if (hit.collider)
            {
                return true;
            }
        }
        return false;
    }

    void Fall()
    {
        moveSpeed = idleSpeed;
        player.playerAnimation.FallAnimation();
        direction += Physics.gravity * 0.005f;
        controller.Move(direction * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit by obstacle");
            //Debug.Log(layerString);
            hasCollided = true;

        }
    }
}
