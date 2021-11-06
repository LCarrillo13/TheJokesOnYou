using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public enum Mode { Race, Survival }
    public enum Map { Day, Night }

    public Mode mode;
    public Map map;

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
