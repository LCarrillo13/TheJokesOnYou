using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Networking
{
    public class MatchManager : NetworkBehaviour
    {
        CustomNetworkManager networkManager;
        public static MatchManager instance = null;
        [SyncVar(hook = nameof(OnReceivedMatchStarted))] public bool matchStarted = false;

        public enum Mode { Race, Survival, TimeTrial }
        public enum Map { Day, Night }

        public Mode mode;
        public static Map map;

        [SerializeField] GameObject dayMap, nightMap;

        Scene currentScene;

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            networkManager = CustomNetworkManager.Instance;
            currentScene = SceneManager.GetActiveScene();
        }

        void OnReceivedMatchStarted(bool _old, bool _new)
        {  
            if (_new) ChooseMode();
        }

        void ChooseMode()
        {
            switch (mode)
            {
                case Mode.Race:
                    networkManager.ServerChangeScene("mode_Race");
                    break;
                case Mode.Survival:
                    networkManager.ServerChangeScene("mode_Survival");
                    break;
                case Mode.TimeTrial:
                    networkManager.ServerChangeScene("mode_TimeTrial");
                    break;
                default:
                    break;
            }
        }

        public void ChooseMap()
        {
            switch (map)
            {
                case Map.Day: // if day map was chosen
                    GameObject dayMapInstance = Instantiate(dayMap);
                    NetworkServer.Spawn(dayMapInstance);
                    break;
                case Map.Night: // if night map was chosen
                    GameObject nightMapInstance = Instantiate(nightMap);
                    NetworkServer.Spawn(nightMapInstance);
                    break;
                default:
                    break;
            }
        }

        [Server]
        public void StartMatch() => matchStarted = true;

        void Update()
        {
            if (currentScene.name != "mode_Survival") return;
        }
    } 
}
