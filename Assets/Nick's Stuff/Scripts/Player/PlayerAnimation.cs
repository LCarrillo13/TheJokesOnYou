using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Player player;
    Animator animator;
    [Tooltip("This controls how smooth the blend between animations is (a lower value means a quicker transition)")]
    [SerializeField] float smoothness;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
    }

    public void IdleClimb() => animator.SetFloat("Speed", 0, smoothness, Time.deltaTime);
    public void SlowClimb() => animator.SetFloat("Speed", 0.25f, smoothness, Time.deltaTime);
    public void FastClimb() => animator.SetFloat("Speed", 0.5f, smoothness, Time.deltaTime);
    public void Fall() => animator.SetFloat("Speed", 1, smoothness, Time.deltaTime);
}
