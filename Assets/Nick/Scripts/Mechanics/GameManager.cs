using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    enum Mode { Race, Survival }

    [SerializeField] Mode mode;

    public void ChangeGameMode(int index) => mode = (Mode)index;

    // different scene is loaded depending on which gamemode is active
    public void StartGame()
    {
        switch (mode)
        {
            case Mode.Race:
                ChangeScene("Race");
                break;
            case Mode.Survival:
                ChangeScene("Survival");
                break;
            default:
                break;
        }
    }

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
