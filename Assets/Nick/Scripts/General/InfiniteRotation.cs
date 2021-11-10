using UnityEngine;

public class InfiniteRotation : MonoBehaviour
{
    [SerializeField] float speed;

    void Update() => transform.Rotate(Vector3.one * speed * Time.deltaTime);
}
