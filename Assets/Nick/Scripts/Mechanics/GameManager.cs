using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    enum Mode { Race, Survival }

    [SerializeField] Mode mode;

    public void ChangeGameMode(int index) => mode = (Mode)index;

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

    void ChangeScene(string name) => SceneManager.LoadScene(name);
}
