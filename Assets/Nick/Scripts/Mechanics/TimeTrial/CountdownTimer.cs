using System.Collections;
using UnityEngine;
using Mirror;
using TMPro;

namespace Networking
{
    public class CountdownTimer : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI countdownText;
        [SyncVar(hook = nameof(OnCountdownChanged))] public int count;

        [Server]
        public IEnumerator Countdown(int seconds)
        {
            count = seconds;

            while (count > 0)
            {
                countdownText.text = count.ToString();
                yield return new WaitForSeconds(1);
                count--;
            }
            countdownText.text = "GO!";
            countdownText.CrossFadeAlpha(0, 2, false);
            RpcAllowMovement();
        }

        public void OnCountdownChanged(int _old, int _new)
        {
            count = _new;
            countdownText.text = count.ToString();

            if (count == 0)
            {
                countdownText.text = "GO!";
                countdownText.CrossFadeAlpha(0, 2, false);
            }
        }

        [ClientRpc]
        public void RpcAllowMovement()
        {
            CustomNetworkManager.Instance.canMove = true;
        }
    } 
}
