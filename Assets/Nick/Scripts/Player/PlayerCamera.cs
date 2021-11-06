using UnityEngine;

public class PlayerCamera : MonoBehaviour
{ 
    public Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;

    // Camera movement is updated at the end of each frame
    void LateUpdate() => MoveCamera();

    // camera smoothly moves to follow the player
    void MoveCamera() => transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
}
