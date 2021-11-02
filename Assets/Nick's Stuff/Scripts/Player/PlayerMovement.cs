using UnityEngine;
using Mirror;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    CharacterController controller;
    [Header("Camera")]
    [SerializeField] GameObject playerCamera;
    [SerializeField] float slowSpeed, fastSpeed, idleSpeed;
    [SerializeField] LayerMask climbableLayer;
    [SerializeField] Vector3 climbableOffset;
    float moveSpeed;
    Vector3 direction;
    RaycastHit hit;
    [Header("Controls")]
    [SerializeField] KeyCode drop;
    [Header("Animation")]
    Animator animator;
    [Tooltip("This controls how smooth the blend between animations is (a lower value means a quicker transition)")]
    [SerializeField] float animationSmoothness;
    [Header("Score")]
    [SerializeField] float score;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // spawns a camera that follows the local player
        if (isLocalPlayer)
        {
            GameObject c = Instantiate(playerCamera);
            c.GetComponent<PlayerCamera>().target = transform;
            c.gameObject.SetActive(true);

            TextMeshProUGUI t = Instantiate(scoreText);
            t.text = score.ToString("0");
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(drop))
        {
            Fall();
        }
        else
        {
            Climb();
        }
    }

    #region Movement
    public void Climb()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        direction = new Vector3(x, y, 0);

        // direction is converted to World Space
        direction = transform.TransformDirection(direction);

        // 'Running'
        if (direction != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = fastSpeed;
            FastClimbAnimation();
        }
        // 'Walking'
        else if (direction != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = slowSpeed;
            ClimbAnimation();
        }
        // Idle
        else if (direction == Vector3.zero)
        {
            moveSpeed = 0;
            IdleAnimation();
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
        FallAnimation();

        // gravity is applied to the player
        direction += Physics.gravity * 0.005f;

        // player falls
        controller.Move(direction * Time.deltaTime);
    }
    #endregion

    #region Animation
    public void IdleAnimation() => animator.SetFloat("Speed", 0, animationSmoothness, Time.deltaTime);
    public void ClimbAnimation() => animator.SetFloat("Speed", 0.25f, animationSmoothness, Time.deltaTime);
    public void FastClimbAnimation() => animator.SetFloat("Speed", 0.5f, animationSmoothness, Time.deltaTime);
    public void FallAnimation() => animator.SetFloat("Speed", 1, animationSmoothness, Time.deltaTime);
    #endregion
}