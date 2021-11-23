using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

namespace Networking
{
    public class NetworkPlayer : NetworkBehaviour
    {
        #region Variables
        Results results;
        [Header("Attributes")]
        [SerializeField] float speed;
        [Header("Customisation")]
        [SerializeField] TextMesh nameTag;
        [SyncVar(hook = nameof(OnNameChanged))] public string playerName;
        [SyncVar(hook = nameof(OnColorChanged))] public Color playerColor = Color.white;
        Material playerMaterialClone;
        [Header("Controls")]
        [SerializeField] List<KeyCode> controls = new List<KeyCode>();
        [SerializeField] List<Transform> positions = new List<Transform>();
        [Header("Teleportation")]
        [SerializeField] Image teleportCooldownSlider;
        [SerializeField] float teleportDelay;
        bool hasTeleported;
        float teleportTimer;
        [Header("Extra")]
        Scene currentScene;
        [SerializeField] Rigidbody rb;
        [SerializeField] Transform cameraMountPoint;
        [SerializeField] GameObject playerCamera;
        [SerializeField] Canvas playerCanvas;
        [SerializeField] GameObject tempCamera;
        [SerializeField] GameObject mobileButton1, mobileButton2, mobileButton3, mobileButton4;
        
        #endregion

        #region Overrides
        // called if we are the local player and NOT a remote player
        public override void OnStartLocalPlayer()
        {
            if(!currentScene.name.StartsWith("mode")) SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);

            // player name and color setup
            string name = PlayerNameInput.DisplayName;
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdPlayerVisuals(name, color);
        }

        // called similarly to Start() for client and host
        public override void OnStartClient() => CustomNetworkManager.AddPlayer(this);

        // called when client or host disconnects
        public override void OnStopClient() => CustomNetworkManager.RemovePlayer(this);
        #endregion

        #region Lobby 

        #region Starting Game
        public void StartMatch()
        {
            if (!isLocalPlayer) return;
            CmdStartMatch();
        }

        [Command]
        public void CmdStartMatch() => MatchManager.instance.StartMatch();
        #endregion

        #region Disconnecting
        public void LeaveMatch()
        {
            if (!isLocalPlayer) return;
            CmdLeaveMatch();
        }

        [Client]
        public void CmdLeaveMatch()
        {
            CustomNetworkManager.Instance.StopClient();
            if (isServer) CustomNetworkManager.Instance.StopHost();
            SceneManager.LoadScene("Menu");
        }
        #endregion

        #region Readying
        public void Ready()
        {
            if (!isLocalPlayer) return;
            CmdReady();
        }

        [Command]
        public void CmdReady()
        {

        }
        #endregion

        #region Results
        public void ReturnToLobby()
        {
            if (!isLocalPlayer) return;
            CmdReturnToLobby();
        }

        [Command]
        public void CmdReturnToLobby()
        {
            CustomNetworkManager.Instance.ServerChangeScene("Lobby");
        }
        #endregion

        #endregion

        #region Player

        void Awake()
        {
            currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "mode_Results") return;
            results = FindObjectOfType<Results>();
        }

        void Start()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty" || currentScene.name == "mode_Results")
            {
                UpdateCursor(CursorLockMode.None, true);
                return;
            }

            if (!isLocalPlayer) return;
            Setup();
        }

        void Update()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty" || currentScene.name == "mode_Results")
            {
                return;
            }

            if (!isLocalPlayer) return;
            Movement();
        }

        #region Player Customisation
        // player info sent to server, then server updates sync vars which handles it on all clients
        [Command]
        public void CmdPlayerVisuals(string name, Color color)
        {
            playerName = name;
            playerColor = color;
        }
        
        void OnNameChanged(string _old, string _new) => nameTag.text = playerName;

        void OnColorChanged(Color _old, Color _new)
        {
            nameTag.color = _new;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _new;
            GetComponent<Renderer>().material = playerMaterialClone;
        }
        #endregion

        #region Player Setup
        public void Setup()
        {
            SetupCamera();
            UpdateCursor(CursorLockMode.Locked, false);
            PopulatePositions();
        }

        // sets up player camera
        public void SetupCamera()
        {
            // create camera
            GameObject c = Instantiate(playerCamera);

            // attach camera
            c.transform.SetParent(cameraMountPoint);
            c.transform.position = cameraMountPoint.position;

            // set canvas to be rendered by this camera
            playerCanvas.gameObject.SetActive(true);
            playerCanvas.worldCamera = c.GetComponent<Camera>();

            // spawn camera
            NetworkServer.Spawn(c);
        }

        // updates cursor lockmode and visibility
        void UpdateCursor(CursorLockMode mode, bool visible)
        {
            Cursor.lockState = mode;
            Cursor.visible = visible;
        }

        // adds the positions used to move to the list
        void PopulatePositions()
        {
            positions[0] = GameObject.Find("Position1").transform;
            positions[1] = GameObject.Find("Position2").transform;
            positions[2] = GameObject.Find("Position3").transform;
            positions[3] = GameObject.Find("Position4").transform;
        }
        #endregion

        #region Player Movement
        public void Movement()
        {
            if (!CustomNetworkManager.Instance.canMove) return;

            AutomaticMovement();
            DesktopTeleport();
            TeleportCooldown();
            PlayerDeath();
        }

        void AutomaticMovement()
        {
            if(currentScene.name != "mode_Race" && currentScene.name != "mode_TimeTrial") return;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //Debug.Log("automatic movement enabled");
            
        }

        // teleports the player to 1 of 4 different positions depending on which key they press (1, 2, 3, 4)
        void DesktopTeleport()
        {
            if (hasTeleported || !CanTeleport()) return;

            if (Input.GetKeyDown(controls[0]))
            {
                rb.useGravity = false;
                transform.position = new Vector3(positions[0].position.x, 3, transform.position.z);
                hasTeleported = true;
                rb.useGravity = true;
            }
            else if (Input.GetKeyDown(controls[1]))
            {
                rb.useGravity = false;
                transform.position = new Vector3(positions[1].position.x, 3, transform.position.z);
                hasTeleported = true;
                rb.useGravity = true;
            }
            else if (Input.GetKeyDown(controls[2]))
            {
                rb.useGravity = false;
                transform.position = new Vector3(positions[2].position.x, 3, transform.position.z);
                hasTeleported = true;
                rb.useGravity = true;
            }
            else if (Input.GetKeyDown(controls[3]))
            {
                rb.useGravity = false;
                transform.position = new Vector3(positions[3].position.x, 3, transform.position.z);
                hasTeleported = true;
                rb.useGravity = true;
            }
        }

        public void TeleportToPosition1()
        {
            if (hasTeleported || !CanTeleport()) return;
            rb.useGravity = false;
            transform.position = new Vector3(positions[0].position.x, 3, transform.position.z);
            hasTeleported = true;
            rb.useGravity = true;
        }

        public void TeleportToPosition2()
        {
            if (hasTeleported || !CanTeleport()) return;
            rb.useGravity = false;
            transform.position = new Vector3(positions[1].position.x, 3, transform.position.z);
            hasTeleported = true;
            rb.useGravity = true;
        }

        public void TeleportToPosition3()
        {
            if (hasTeleported || !CanTeleport()) return;
            rb.useGravity = false;
            transform.position = new Vector3(positions[2].position.x, 3, transform.position.z);
            hasTeleported = true;
            rb.useGravity = true;
        }

        public void TeleportToPosition4()
        {
            if (hasTeleported || !CanTeleport()) return;
            rb.useGravity = false;
            transform.position = new Vector3(positions[3].position.x, 3, transform.position.z);
            hasTeleported = true;
            rb.useGravity = true;
        }

        // players can only teleport every few seconds
        void TeleportCooldown()
        {
            if (!hasTeleported) return;

            if (teleportTimer > teleportDelay)
            {
                hasTeleported = false;
                teleportTimer = 0;
            }
            else
            {
                teleportTimer += Time.deltaTime;
                teleportCooldownSlider.fillAmount = teleportTimer;
            }
        }

        // checks if the player can telport by detecting if they are on the platform or not
        bool CanTeleport()
        {        
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3)) 
            {
                if (hit.collider) return true;
            }
            return false;
        }

        // player gets respawned if they fall off the platforms
        void PlayerDeath()
        {
            if (transform.position.y < -5)
            {
                // choose random starting position to respawn
                int index = Random.Range(0, positions.Count);
                // respawn player back at the start
                transform.position = positions[index].transform.position;
            }
        }
        #endregion

        #endregion
    }
}