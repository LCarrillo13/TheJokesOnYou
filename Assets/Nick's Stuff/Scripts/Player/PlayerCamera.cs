using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    void LateUpdate() => NormalCamera();
    void NormalCamera() => transform.position = Vector3.Lerp(transform.position, player.position + offset, speed * Time.deltaTime);
}
