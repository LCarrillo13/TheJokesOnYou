using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public enum GameMode { Race, Survival }
    [SerializeField] GameMode currentGameMode;
    [SerializeField] int gameModeIndex;
    public bool hasStarted;
    [SerializeField] GameObject matchUI, gameUI, resultsUI;
    [SerializeField] GameObject[] maps;
    [SerializeField] int mapIndex;
    [SerializeField] Slider timeLimitSlider, scoreLimitSlider;
    [SerializeField] TMP_Dropdown gameModeDropdown, mapDropdown;
    public float timeLimit, scoreLimit;
    [SerializeField] TextMeshProUGUI timeLimitText, scoreLimitText;
    [Header("During Game")]
    [SerializeField] TextMeshProUGUI timeText;

    void Start()
    {
        timeLimit = timeLimitSlider.value;
        timeLimitText.text = "Time Limit : " + timeLimit.ToString("0");
        scoreLimit = scoreLimitSlider.value;
        scoreLimitText.text = "Score Limit : " + scoreLimit.ToString("0");
        
        resultsUI.SetActive(false);
    }

    void Update()
    {
        if (hasStarted)
        {
            UpdateUI();
            StartGame();          
        }

        GameModeCheck();
    }

    [ClientRpc]
    void UpdateUI()
    {
        matchUI.SetActive(false);
        gameUI.SetActive(true);
    }

    #region Live Match Settings
    void StartGame()
    {     
        if (timeLimit <= 0)
        {
            GameEnd();         
        }
        else
        {
            timeLimit -= Time.deltaTime;
            timeText.text = timeLimit.ToString("0");

            
        }
    }

    void GameModeCheck()
    {
        switch (gameModeIndex)
        {
            case 0:
                currentGameMode = GameMode.Race;
                break;
            case 1:
                currentGameMode = GameMode.Survival;
                break;
            default:
                break;
        }
    }
    #endregion

    #region Change Match Settings
    public void ChangeTimeLimit(float amount)
    {
        amount = timeLimitSlider.value;
        timeLimit = amount;
        timeLimitText.text = "Time Limit : " + amount.ToString("0");
    }

    public void ChangeScoreLimit(float amount)
    {
        amount = scoreLimitSlider.value;
        scoreLimit = amount;
        scoreLimitText.text = "Score Limit : " + amount.ToString("0");
    }

    public void ChangeGameMode(float index)
    {
        index = gameModeDropdown.value;
        gameModeIndex = (int)index;
    }

    public void ChangeMap(int index)
    {
        index = mapDropdown.value;
        mapIndex = (int)index;
    }
    #endregion

    public void GameEnd()
    {
        gameUI.SetActive(false);
        resultsUI.SetActive(true);
    }
}
