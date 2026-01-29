using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SimpleWallGrab2D : MonoBehaviour, IWallGrabAbility
{
    [Header("General")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private SimpleJump2D simpleJump2D;
    [SerializeField] private SimpleDash2D simpleDash2D;

    [Header("Detection")]
    [SerializeField] private float checkDistance = 0.08f; // collider half-width + margin kullanýlacak
    [SerializeField] private float verticalOffset = 0.0f; // ray origin dikey ofset

    [Header("Behaviour")]
    [SerializeField] private float wallSlideSpeed = 1.5f; // aþaðýya maksimum düþüþ hýzý
    [SerializeField] private float leaveGraceTime = 0.1f; // yön ters basýlýnca býrakmadan önce süre
    [SerializeField] private float wallIgnoreTime = 5f; // zýpladýktan sonra ignore süresi
    [SerializeField] private Vector2 wallJumpForce = new Vector2(8f, 12f); // x: yatay, y: dikey
    [SerializeField] private float maxGrabTime = 0f; // 0 = sýnýrsýz

    private float leaveTimer = 0f;

    private Rigidbody2D rb;
    private Collider2D col;
    private IGroundChecker ground;
    private bool isGrabbing;
    private float grabTimer;
    private int wallDir = 0; // -1 = left wall, +1 = right wall
    private int lastWallDir = 0;      // Son duvar yönü (-1 sol, +1 sað)
    private float wallIgnoreTimer = 0f;

    public bool IsGrabbing => isGrabbing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        ground = GetComponent<IGroundChecker>();
    }

    /// <summary>
    /// Called from CharacterController2D.FixedUpdate
    /// Returns true if it consumed the jumpPressed (i.e. wall-jump performed)
    /// </summary>
    public bool TryWallGrab(float horizontalInput, bool jumpPressed, bool jumpHeld)
    {
        // Update ignore timer
        if (wallIgnoreTimer > 0f)
            wallIgnoreTimer -= Time.fixedDeltaTime;

        if (ground != null && ground.IsGrounded)
        {
            ReleaseGrab();
            wallIgnoreTimer = 0f;
            return false;
        }

        // Raycast setup
        Vector2 center = col.bounds.center;
        float halfWidth = col.bounds.extents.x;
        Vector2 origin = new Vector2(center.x, center.y + verticalOffset);
        float dist = halfWidth + checkDistance;

        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, dist, wallLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, dist, wallLayer);

        bool touchingRight = hitRight.collider != null;
        bool touchingLeft = hitLeft.collider != null;

        int side = 0;
        if (touchingRight) side = +1;
        else if (touchingLeft) side = -1;

        // Eðer ignore timer aktifse ve duvar aynýysa -> ignore et
        if (wallIgnoreTimer > 0f && side == lastWallDir)
        {
            side = 0; // bu duvarý yok say
        }

        if (side == 0)
        {
            ReleaseGrab();
            return false;
        }

        // leaveTimer mantýðý
        if (isGrabbing)
        {
            if (Mathf.Abs(horizontalInput) > 0.1f && Mathf.Sign(horizontalInput) != side)
            {
                leaveTimer += Time.fixedDeltaTime;
                if (leaveTimer >= leaveGraceTime)
                {
                    ReleaseGrab();
                    return false;
                }
            }
            else
            {
                leaveTimer = 0f;
            }
        }
        else
        {
            leaveTimer = 0f;
            StartGrab(side);
        }

        // vertical speed clamp
        if (rb.velocity.y < -wallSlideSpeed)
            rb.velocity = new Vector2(0f, -wallSlideSpeed);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);

        if (maxGrabTime > 0f)
        {
            grabTimer += Time.fixedDeltaTime;
            if (grabTimer >= maxGrabTime)
                ReleaseGrab();
        }

        // Wall jump
        if (jumpPressed)
        {
            PerformWallJump(side);
            SoundManager.Instance.PlayWallJump();

            // Jump sonrasý ayný duvara hemen tutunmayý engelle
            lastWallDir = side;
            wallIgnoreTimer = wallIgnoreTime;

            return true;
        }

        return false;
    }


    private void StartGrab(int side)
    {
        isGrabbing = true;
        wallDir = side;
        grabTimer = 0f;
        PlayerAnimationController.Instance.SetWallGrab(true);
        // optional: play animation / set animator param
        // Debug.Log("Started wall grab side: " + side);

        simpleJump2D.ResetAirJumps();
        simpleDash2D.ResetDash();
    }

    private void ReleaseGrab()
    {
        if (!isGrabbing) return;
        isGrabbing = false;
        wallDir = 0;
        grabTimer = 0f;
        PlayerAnimationController.Instance.SetWallGrab(false);
    }

    private void PerformWallJump(int side)
    {
        // side = +1 means wall is at right side -> we want to jump left (negative)
        int jumpDir = -side;
        // Set velocity directly for consistent behaviour
        Vector2 v = rb.velocity;
        v.x = jumpDir * wallJumpForce.x;
        v.y = 0f; // reset vertical then add impulse
        rb.velocity = v;

        rb.AddForce(Vector2.up * wallJumpForce.y, ForceMode2D.Impulse);

        // Release after jump
        PlayerAnimationController.Instance.SetJumping(true);
        ReleaseGrab();
    }
}
