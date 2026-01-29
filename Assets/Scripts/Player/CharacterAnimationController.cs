using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance { get; private set; }

    private Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Koþma animasyonunu aç/kapat
    /// </summary>
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    /// <summary>
    /// Zýplama animasyonu
    /// </summary>
    public void SetJumping(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }

    /// <summary>
    /// Dash animasyonu tetikle
    /// </summary>
    public void PlayDash()
    {
        animator.SetTrigger("dash");
    }

    /// <summary>
    /// Wall grab animasyonu aç/kapat
    /// </summary>
    public void SetWallGrab(bool isGrabbing)
    {
        animator.SetBool("isWallGrabbing", isGrabbing);
    }

    public void SetDigging(bool isDigging)
    {
        animator.SetBool("isDigging", isDigging);
    }
}
