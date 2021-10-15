using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    // Camera movement is updated at the end of each frame
    void LateUpdate() => NormalCamera();

    // camera smoothly moves to follow the player
    void NormalCamera() => transform.position = Vector3.Lerp(transform.position, player.position + offset, speed * Time.deltaTime);
}
