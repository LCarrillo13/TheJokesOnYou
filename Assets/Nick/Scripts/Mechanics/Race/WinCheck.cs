using UnityEngine;
using Networking;

namespace Networking
{
    public class WinCheck : MonoBehaviour
    {
        CustomNetworkManager networkManager;
        public static string winner = string.Empty;

        void Awake() => networkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                winner = collision.collider.GetComponentInChildren<TextMesh>().text;
                networkManager.ServerChangeScene("mode_Results");
            }
        }
    } 
}
