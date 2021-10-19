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

    public void IdleAnimation() => animator.SetFloat("Speed", 0, smoothness, Time.deltaTime);
    public void SlowClimbAnimation() => animator.SetFloat("Speed", 0.33f, smoothness, Time.deltaTime);
    public void FastClimbAnimation() => animator.SetFloat("Speed", 0.66f, smoothness, Time.deltaTime);
    public void FallAnimation() => animator.SetFloat("Speed", 1, smoothness, Time.deltaTime);
}
