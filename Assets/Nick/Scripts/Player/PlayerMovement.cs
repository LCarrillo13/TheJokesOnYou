using System.Collections.Generic;
using UnityEngine.SceneManagement;  
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    #region Variables
    [Header("Attributes")]
    [SerializeField] float speed;
    public TextMesh nameTag;
    [SyncVar(hook = nameof(OnColorChanged))] 
    public Color playerColor = Color.white;
    Material playerMaterialClone;
    [Header("Controls")]
    [SerializeField] List<KeyCode> controls = new List<KeyCode>();
    [SerializeField] List<Transform> positions = new List<Transform>();
    [Header("Teleportation")]
    [SerializeField] float teleportDelay;
    bool hasTeleported;
    float teleportTimer;
    //[SerializeField] Color charged, recharging;
    //[SerializeField] Material playerMaterial;
    [Header("Climbable Check")]
    [SerializeField] Vector3 raycastOffset;
    [SerializeField] LayerMask climbableLayer;
    RaycastHit hit;
    Scene scene;
    [Header("Camera")]
    [SerializeField] GameObject playerCameraPrefab;
    public GameObject tempCamera;
    
    
    
    #endregion

<<<<<<< HEAD
    void Awake()
    {
        manager = GameObject.Find("Manager - Game").GetComponent<GameManager>();
        nameTag = GetComponentInChildren<TextMesh>();
        if(isLocalPlayer)
        {
         
            GetComponentInChildren<Canvas>().gameObject.SetActive(true);
            
        }
    }
=======
    void Awake() => nameTag = GetComponentInChildren<TextMesh>();
>>>>>>> origin/nick

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        HideLockCursor();
        SpawnCamera();
        PopulatePositions();

        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(PlayerNameInput.DisplayName, color);
    }

    void Update()
    {
        if (scene.name == "map_Race") AutomaticMovement();
        if (!hasTeleported) Teleport();
        TeleportCooldown();
    }

    void OnNameChanged(string _Old, string _New) => nameTag.text = PlayerNameInput.DisplayName;

    void OnColorChanged(Color _Old, Color _New)
    {
        nameTag.color = _New;
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        nameTag.text = _name;
        playerColor = _col;
    }

    // players are always moving upwards if they are touching a 'climbable' layer
    void AutomaticMovement()
    {
        // Debug.DrawRay(transform.position + raycastOffset, Vector3.forward, Color.red, 5);
        if (Physics.Raycast(transform.position + raycastOffset, Vector3.forward, out hit, 5, climbableLayer))
        {
            if (hit.collider) transform.Translate(Vector3.up * speed * Time.deltaTime);
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
                //playerMaterial.color = charged;
            }
            else
            {
                teleportTimer += Time.deltaTime;
                //playerMaterial.color = recharging;
            }
        }
    }

    // hides and locks the cursor to center of screen
    void HideLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // gives the player their own camera
    void SpawnCamera()
    {
        GameObject cameraInstance = Instantiate(playerCameraPrefab);
        NetworkServer.Spawn(cameraInstance);
        cameraInstance.GetComponent<PlayerCamera>().target = this.transform;
        tempCamera = cameraInstance;
    }
    // adds the positions used to move to the list
    void PopulatePositions()
    {
        positions[0] = GameObject.Find("Position1").transform;
        positions[1] = GameObject.Find("Position2").transform;
        positions[2] = GameObject.Find("Position3").transform;
        positions[3] = GameObject.Find("Position4").transform;
    }
    
}
