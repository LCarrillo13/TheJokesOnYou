using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;
using TMPro;

namespace Networking
{
    public class NetworkPlayer : NetworkBehaviour
    {
        #region Variables
        [Header("Attributes")]
        [SerializeField] float speed;
        [Header("Player Customisation")]
        [SerializeField] TextMesh nameTag;
        [SyncVar(hook = nameof(OnNameChanged))] public string playerName;
        [SyncVar(hook = nameof(OnColorChanged))] public Color playerColor = Color.white;
        Material playerMaterialClone;
        [Header("Controls")]
        [SerializeField] List<KeyCode> controls = new List<KeyCode>();
        [SerializeField] List<Transform> positions = new List<Transform>();
        [Header("Teleportation")]
        [SerializeField] float teleportDelay;
        bool hasTeleported;
        float teleportTimer;
        [SerializeField] Rigidbody rb;
        [Header("Climbable Check")]
        [SerializeField] Vector3 raycastOffset;
        [SerializeField] LayerMask climbableLayer;
        RaycastHit hit;
        Scene currentScene;
        [SerializeField] Transform cameraMountPoint;
        [SerializeField] GameObject playerCamera;
        #endregion

        #region Overrides
        // called if we are the local player and NOT a remote player
        public override void OnStartLocalPlayer()
        {
            Scene scene = SceneManager.GetActiveScene();
            if(!scene.name.StartsWith("mode")) SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);

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

        #endregion

        #region Player

        void Awake() => currentScene = SceneManager.GetActiveScene();

        void Start()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty") return;

            if (!isLocalPlayer) return;
            Setup();
        }

        void Update()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty") return;

            if (!isLocalPlayer) return;
            Movement();
        }

        #region Player Customation
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
            DisableCursor();
            PopulatePositions();
            CurrentScene();
        }

        void SetupCamera()
        {
            // spawn camera for this player
            GameObject playerCameraInstance = Instantiate(playerCamera);
            NetworkServer.Spawn(playerCameraInstance);

            // attach camera to this player
            playerCameraInstance.transform.SetParent(cameraMountPoint);
            playerCameraInstance.transform.position = cameraMountPoint.position;
        }

        // hides and locks the cursor to center of screen
        void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // adds the positions used to move to the list
        void PopulatePositions()
        {
            positions[0] = GameObject.Find("Position1").transform;
            positions[1] = GameObject.Find("Position2").transform;
            positions[2] = GameObject.Find("Position3").transform;
            positions[3] = GameObject.Find("Position4").transform;
        }

        // gets the current scene
        void CurrentScene()
        {
            currentScene = SceneManager.GetActiveScene();
        }
        #endregion

        #region Player Movement
        public void Movement()
        {
            AutomaticMovement();
            Teleport();
            TeleportCooldown();
            PlayerDeath();
        }

        void AutomaticMovement()
        {
            if (currentScene.name != "mode_Race") return;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // teleports the player to 1 of 4 different positions depending on which key they press (1, 2, 3, 4)
        void Teleport()
        {
            if (hasTeleported) return;

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

        // players can only teleport every few seconds
        void TeleportCooldown()
        {
            if (!hasTeleported) return;

            if (teleportTimer > teleportDelay)
            {
                hasTeleported = false;
                teleportTimer = 0;
            }
            else teleportTimer += Time.deltaTime;
        }

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