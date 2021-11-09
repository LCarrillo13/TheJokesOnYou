using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public enum Mode { Race, Survival, Mode3 }
    public enum Map { Day, Night }

    public static Mode mode;
    public static Map map;

    public Texture dayTexture, nightTexture;

    Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        // spawns a specific map depending on what map is selected
        if (scene.name != "Room")
        {
            CreateMap();
        }
    }

    [Command]
    void CreateMap()
    {
        MeshRenderer temp = GameObject.Find("Quad - Sky").GetComponent<MeshRenderer>();
        if (map == Map.Day) temp.material.mainTexture = dayTexture;
        else if (map == Map.Night) temp.material.mainTexture = nightTexture;
    }

    [Command]
    public void ChangeGameMode(int index) => mode = (Mode)index;

    [Command]
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
