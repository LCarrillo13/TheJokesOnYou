using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    enum Mode { Race, Survival }

    [SerializeField] Mode mode;

    public void ChangeGameMode(int index) => mode = (Mode)index;

    public void StartGame()
    {

    }
}
