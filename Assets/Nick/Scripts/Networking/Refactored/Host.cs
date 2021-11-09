using UnityEngine;

public class Host : MonoBehaviour
{
    [SerializeField] NetworkManagerLobby networkManager = null;
    [SerializeField] GameObject landingPagePanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }
}
