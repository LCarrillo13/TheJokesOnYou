using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public enum Mode { Race, Survival }
    public enum Map { Day, Night }

    public static Mode mode;
    public static Map map;

    public Texture dayMap, nightMap;

    Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        // spawns a specific map depending on what map is selected
        if (scene.name != "Lobby" || scene.name != "Room")
        {
            MeshRenderer temp = GameObject.Find("Quad - Sky").GetComponent<MeshRenderer>();

            if (map == Map.Day)
            {
                temp.material.mainTexture = dayMap;
            }
            else if (map == Map.Night)
            {
                temp.material.mainTexture = nightMap;
            }
        }


    }

    public void ChangeGameMode(int index) => mode = (Mode)index;
    public void ChangeMap(int index) => map = (Map)index;

    // changes the scene for all clients
    public void ChangeScene(string name) => SceneManager.LoadScene(name);

    // loads the results scene for all clients
    [ClientRpc]
    public void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ChangeScene("Results");
    }
}
