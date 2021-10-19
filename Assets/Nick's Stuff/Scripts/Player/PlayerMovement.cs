using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    CharacterController controller;
    [SerializeField] float slowSpeed;
    [SerializeField] float fastSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] Vector3 climbableOffset;
    float moveSpeed;
    Vector3 direction;
    RaycastHit hit;
    [Header("Controls")]
    [SerializeField] KeyCode drop;

    void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(drop))
        {
            Fall();
        }
        else
        {
            Climb();
        }
    }

    public void Climb()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        direction = new Vector3(x, y, 0);

        // direction is converted to World Space
        direction = transform.TransformDirection(direction);

        if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastSpeed;
            player.playerAnimation.FastClimb();
        }
        else if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = slowSpeed;
            player.playerAnimation.SlowClimb();
        }
        // ELSE IF player is not moving
        else if (direction == Vector3.zero)
        {
            moveSpeed = 0;
            player.playerAnimation.IdleClimb();
        }

        // moves the character controller
        direction *= moveSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    // checks if surface in front of player is climbable
    bool isClimbable()
    {
        Debug.DrawRay(transform.position + climbableOffset, Vector3.forward, Color.red, 2);

        // ray is casted forward from player and attempts to hit something on the 'climbableLayer'
        if (Physics.Raycast(transform.position + climbableOffset, Vector3.forward, out hit, 2, climbableLayer))
        {
            if (hit.collider) return true;
        }
        return false;
    }

    void Fall()
    {
        // player becomes idle
        moveSpeed = 0;
        player.playerAnimation.Fall();

        // gravity is applied to the player
        direction += Physics.gravity * 0.005f;

        // player falls
        controller.Move(direction * Time.deltaTime);
    }
}