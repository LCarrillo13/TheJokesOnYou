using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed;
    [SerializeField] bool canMove;
    [Header("Controls")]
    [SerializeField] List<KeyCode> controls = new List<KeyCode>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [Header("Teleportation")]
    [SerializeField] TextMeshProUGUI teleportTimerText;
    [SerializeField] float teleportDelay;
    bool hasTeleported;
    float teleportTimer;
    [Header("Climbable Check")]
    [SerializeField] Camera playerCamera;
    [SerializeField] LayerMask climbableLayer;
    RaycastHit hit;
    Rigidbody rb;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Start() => HideLockCursor();

    void Update()
    {
        // makes sure each client controls their own player
        // if (!isLocalPlayer) return;
        if (canMove) AutomaticMovement();
        if (!hasTeleported) Teleport();
        TeleportCooldown();
        CheckForWin();
    }

    // players are always moving upwards if they are touching a 'climbable' layer
    void AutomaticMovement()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, climbableLayer))
        {
            if (hit.collider)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }
    }

    // teleports the player to 1 of 4 different positions depending on which key they press (1, 2, 3, 4)
    void Teleport()
    {
        if (Input.GetKeyDown(controls[0]))
        {
            transform.position = new Vector3(positions[0].position.x, transform.position.y, -3);
            hasTeleported = true;
        }
        else if (Input.GetKeyDown(controls[1]))
        {
            transform.position = new Vector3(positions[1].position.x, transform.position.y, -3);
            hasTeleported = true;
        }
        else if (Input.GetKeyDown(controls[2]))
        {
            transform.position = new Vector3(positions[2].position.x, transform.position.y, -3);
            hasTeleported = true;
        }
        else if (Input.GetKeyDown(controls[3]))
        {
            transform.position = new Vector3(positions[3].position.x, transform.position.y, -3);
            hasTeleported = true;
        }
    }

    // players can only teleport every few seconds
    void TeleportCooldown()
    {
        if (hasTeleported)
        {
            if (teleportTimer > teleportDelay)
            {
                hasTeleported = false;
                teleportTimer = 0;
                teleportTimerText.text = "Teleportation Ready!";
            }
            else
            {
                teleportTimer += Time.deltaTime;
                teleportTimerText.text = "Recharging teleport!";
            }
        }
    }

    // hides and locks the cursor to center of screen
    void HideLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // checks if player has reach the finish line in race mode
    void CheckForWin()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            if (hit.collider.CompareTag("Finish"))
            {
                GameEnd();
            }
        }
    }

    void GameEnd()
    {
        print(this.gameObject.name + " has won the game!");
    }
}
