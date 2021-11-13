using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField = null;
    [SerializeField] Button confirmButton = null;
    // 'static' used so we can access a player's name anywhere
    public static string DisplayName { get; private set; }
    // 'const' used to prevent modification of this string
    const string PlayerPrefsNameKey = "Noob";

    void Start() => SetupInputField();

    void SetupInputField()
    {
        // if player doesn't already have a saved name, skip this method
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;

        // player name is set to saved name
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    // !string.IsNullOrEmpty - checks that the string isn't null (nothing was typed) or empty ("")
    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrEmpty(name)) confirmButton.interactable = true;
        else confirmButton.interactable = false;
    }

    // sets inputted name to DisplayName and saves it to PlayerPrefsNameKey
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text; 
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}