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
}
