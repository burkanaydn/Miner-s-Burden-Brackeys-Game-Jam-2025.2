using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyMover2D : MonoBehaviour, IMover
{
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 70f;
    [SerializeField] private bool flipSpriteOnFace = true;
    [SerializeField] private Transform spriteRoot;

    private Rigidbody2D rb;
    private BagWeightController weightController;
    private IGroundChecker ground;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRoot == null) spriteRoot = transform;
        weightController = GetComponent<BagWeightController>();
        ground = GetComponent<IGroundChecker>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // Animasyon için koþma durumu
        if (Mathf.Abs(h) > 0.01f && ground.IsGrounded)
            PlayerAnimationController.Instance.SetRunning(true);
        else
            PlayerAnimationController.Instance.SetRunning(false);

        Move(h, Time.fixedDeltaTime);
        Face(h);
    }

    public void Move(float horizontal, float fixedDelta)
    {
        float moveSpeed = weightController != null ? weightController.CurrentSpeed : 5f;
        float target = horizontal * moveSpeed;
        float speedDiff = target - rb.velocity.x;
        float accelRate = (Mathf.Abs(target) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Clamp(speedDiff, -accelRate * fixedDelta, accelRate * fixedDelta);

        rb.velocity = new Vector2(rb.velocity.x + movement, rb.velocity.y);
    }

    public void Face(float dirX)
    {
        if (!flipSpriteOnFace || Mathf.Abs(dirX) < 0.01f) return;
        Vector3 s = spriteRoot.localScale;
        s.x = Mathf.Sign(dirX) * Mathf.Abs(s.x);
        spriteRoot.localScale = s;
    }
}
