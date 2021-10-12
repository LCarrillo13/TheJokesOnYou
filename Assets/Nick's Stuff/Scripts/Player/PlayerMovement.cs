using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    CharacterController controller;
    [SerializeField] float idleSpeed;
    [SerializeField] float slowClimbSpeed;
    [SerializeField] float fastClimbSpeed;
    float moveSpeed;
    Vector3 direction;

    void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!player.canMove) return; // Guard Clause prevents unnecessary update cycles until the if statement returns false
        Move();
    }

    void Move()
    {
        float moveZ = Input.GetAxis("Vertical");
        direction = new Vector3(0, 0, moveZ);
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

        //if (!controller.isGrounded) direction += Physics.gravity;

        controller.Move(direction * Time.deltaTime);
    }
}
