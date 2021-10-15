using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    CharacterController controller;
    [SerializeField] float idleSpeed;
    [SerializeField] float slowClimbSpeed;
    [SerializeField] float fastClimbSpeed;
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] Vector3 raycastOffset;
    float moveSpeed;
    Vector3 direction;
    RaycastHit hit;

    void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // IF space button held down AND surface in front of them is climbable
        if (Input.GetKey(KeyCode.Space) && Climbable())
        {
            // player can climb
            Move();
        }
        // ELSE the player will fall
        else Fall();
    }

    void Move()
    {
        // vertical and horizontal inputs
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        // puts inputs into Vector3
        direction = new Vector3(moveX, moveY, 0);

        // the Vector3 is changed to World Space
        direction = transform.TransformDirection(direction);

        // IF player is moving AND left shift button is held down
        if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            // player climbs fast
            moveSpeed = fastClimbSpeed;
            player.playerAnimation.FastClimbAnimation();
        }
        // ELSE IF player is moving AND left shift button isn't held down
        else if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            // player climbs normally
            moveSpeed = slowClimbSpeed;
            player.playerAnimation.SlowClimbAnimation();
        }
        // ELSE IF player is not moving
        else if (direction == Vector3.zero)
        {
            // player is idle
            moveSpeed = idleSpeed;
            player.playerAnimation.IdleAnimation();
        }

        // makes player move at correct speed
        direction *= moveSpeed;

        // moves the character controller
        controller.Move(direction * Time.deltaTime);
    }

    // checks if surface in front of player is climbable
    bool Climbable()
    {
        #if UNITY_EDITOR
        Debug.DrawRay(transform.position + raycastOffset, Vector3.forward, Color.red, 2);
        #endif

        // ray is casted forward from player and attempts to hit something on the 'climbableLayer'
        if (Physics.Raycast(transform.position + raycastOffset, Vector3.forward, out hit, 2, climbableLayer))
        {
            // if it hits something
            if (hit.collider)
            {
                // player can climb here
                return true;
            }
        }
        // player can't climb here
        return false;
    }

    void Fall()
    {
        // player becomes idle
        moveSpeed = idleSpeed;
        player.playerAnimation.FallAnimation();

        // gravity is applied to the player
        direction += Physics.gravity * 0.005f;

        // player falls
        controller.Move(direction * Time.deltaTime);
    }
}
