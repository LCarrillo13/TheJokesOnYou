using System.Collections;
using UnityEngine;
using Mirror;
using TMPro;

namespace Networking
{
    public class Countdown : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI countdownText;
        [SyncVar(hook = nameof(OnCountdownChanged))] public int count;

        [Server]
        public IEnumerator CountingDown(int seconds)
        {
            count = seconds;

            while (count > 0)
            {       
                yield return new WaitForSeconds(1);
                count--;
            }

            RpcAllowMovement();
        }

        public void OnCountdownChanged(int _old, int _new)
        {
            if (count == 0)
            {
                countdownText.text = "GO!";
                countdownText.CrossFadeAlpha(0, 2, false);
            }
            else
            {
                count = _new;
                countdownText.text = count.ToString();
            }
        }

        [ClientRpc]
        public void RpcAllowMovement() => CustomNetworkManager.Instance.canMove = true;
    } 
}
