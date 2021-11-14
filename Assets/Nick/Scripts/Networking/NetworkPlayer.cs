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
        public TextMesh nameTag;
        [SyncVar(hook = nameof(OnColorChanged))] public Color playerColor = Color.white;
        Material playerMaterialClone;
        [Header("Controls")]
        [SerializeField] List<KeyCode> controls = new List<KeyCode>();
        [SerializeField] List<Transform> positions = new List<Transform>();
        [Header("Teleportation")]
        [SerializeField] float teleportDelay;
        bool hasTeleported;
        float teleportTimer;
        [Header("Climbable Check")]
        [SerializeField] Vector3 raycastOffset;
        [SerializeField] LayerMask climbableLayer;
        RaycastHit hit;
        Scene scene;
        [SerializeField] GameObject cam;
        #endregion

        #region Overrides
        // called if we are the local player and NOT a remote player
        public override void OnStartLocalPlayer()
        {
            Scene scene = SceneManager.GetActiveScene();

            if(!scene.name.StartsWith("mode")) SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
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

        #region Player Controller
        void Awake()
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Lobby") return;

            if (!isLocalPlayer) return;
            nameTag = GetComponentInChildren<TextMesh>();
        }

        void Start()
        {
            if (scene.name == "Lobby") return;
            if (!isLocalPlayer) return;
            CmdSetup();
        }

        void Update()
        {
            if (scene.name == "Lobby") return;
            if (!isLocalPlayer) return;
            CmdMovement();
        }

        [Command]
        public void CmdSetup()
        {
            RpcDisableCursor();
            RpcPopulatePositions();
            RpcCurrentScene();
            RpcSetupCamera();

            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdPlayerVisuals(PlayerNameInput.DisplayName, color);
        }

        // hides and locks the cursor to center of screen
        [ClientRpc]
        void RpcDisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        [ClientRpc]
        // adds the positions used to move to the list
        void RpcPopulatePositions()
        {
            positions[0] = GameObject.Find("Position1").transform;
            positions[1] = GameObject.Find("Position2").transform;
            positions[2] = GameObject.Find("Position3").transform;
            positions[3] = GameObject.Find("Position4").transform;
        }

        [ClientRpc]
        // gets the current scene
        void RpcCurrentScene()
        {
            scene = SceneManager.GetActiveScene();
        }

        [ClientRpc]
        void RpcSetupCamera()
        {
            cam.SetActive(true);
        }

        [Command]
        public void CmdPlayerVisuals(string name, Color color)
        {
            // player info sent to server, then server updates sync vars which handles it on all clients
            nameTag.text = name;
            playerColor = color;
        }

        void OnNameChanged(string _old, string _new)
        {
            nameTag.text = PlayerNameInput.DisplayName;
        }
        void OnColorChanged(Color _old, Color _new)
        {
            nameTag.color = _new;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _new;
            GetComponent<Renderer>().material = playerMaterialClone;
        }
        #endregion

        #region Movement
        [Command]
        public void CmdMovement()
        {
            RpcAutomaticMovement();
            RpcTeleport();
            RpcTeleportCooldown();
        }

        // players are always moving upwards if they are touching a 'climbable' layer
        [ClientRpc]
        void RpcAutomaticMovement()
        {
            if (scene.name != "map_Race") return;

            // Debug.DrawRay(transform.position + raycastOffset, Vector3.forward, Color.red, 5);
            if (Physics.Raycast(transform.position + raycastOffset, Vector3.forward, out hit, 5, climbableLayer))
            {
                if (hit.collider) transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
        }

        // teleports the player to 1 of 4 different positions depending on which key they press (1, 2, 3, 4)
        [ClientRpc]
        void RpcTeleport()
        {
            if (hasTeleported) return;

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
        [ClientRpc]
        void RpcTeleportCooldown()
        {
            if (!hasTeleported) return;

            if (teleportTimer > teleportDelay)
            {
                hasTeleported = false;
                teleportTimer = 0;
            }
            else teleportTimer += Time.deltaTime;
        }

        #endregion
    }
}