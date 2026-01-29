using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("References (boþ býrakýlýrsa otomatik bulunur)")]
    [SerializeField] private MonoBehaviour inputProviderBehaviour;
    [SerializeField] private MonoBehaviour moverBehaviour;
    [SerializeField] private MonoBehaviour groundCheckerBehaviour;
    [SerializeField] private MonoBehaviour jumpAbilityBehaviour;
    [SerializeField] private MonoBehaviour digAbilityBehaviour;
    [SerializeField] private MonoBehaviour dashAbilityBehaviour;
    [SerializeField] private MonoBehaviour wallGrabAbilityBehaviour;

    private IInputProvider input;
    private IMover mover;
    private IGroundChecker ground;
    private IJumpAbility jumper;
    private IDigAbility digger;
    private IDashAbility dasher;
    private IWallGrabAbility wallGrabber;
    private Rigidbody2D rb;

    private float cachedHorizontal;
    private bool cachedJumpPressed;
    private bool cachedJumpHeld;
    private bool cachedDashPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        input = (inputProviderBehaviour as IInputProvider) ?? GetComponent<IInputProvider>();
        mover = (moverBehaviour as IMover) ?? GetComponent<IMover>();
        ground = (groundCheckerBehaviour as IGroundChecker) ?? GetComponent<IGroundChecker>();
        jumper = (jumpAbilityBehaviour as IJumpAbility) ?? GetComponent<IJumpAbility>();
        digger = (digAbilityBehaviour as IDigAbility) ?? GetComponent<IDigAbility>();
        dasher = (dashAbilityBehaviour as IDashAbility) ?? GetComponent<IDashAbility>();
        wallGrabber = (wallGrabAbilityBehaviour as IWallGrabAbility) ?? GetComponent<IWallGrabAbility>();

        if (input == null) Debug.LogError("IInputProvider eksik!");
        if (mover == null) Debug.LogError("IMover eksik!");
        if (ground == null) Debug.LogError("IGroundChecker eksik!");
        if (jumper == null) Debug.LogError("IJumpAbility eksik!");
        if (digger == null) Debug.LogError("IDigAbility eksik!");
        if (dasher == null) Debug.LogError("IDashAbility eksik!");
        if (wallGrabber == null) Debug.LogError("IWallGrabAbility eksik!");


    }

    void Update()
    {
        if (input == null) return;
        cachedHorizontal = input.Horizontal;
        cachedJumpPressed = cachedJumpPressed || input.JumpPressed; 
        cachedJumpHeld = input.JumpHeld;
        cachedDashPressed = cachedDashPressed || input.DashPressed;
    }

    void FixedUpdate()
    {
        ground?.UpdateGrounded();

        mover?.Move(cachedHorizontal, Time.fixedDeltaTime);
        mover?.Face(cachedHorizontal);

        bool consumedByWall = false;
        if (wallGrabber != null)
        {
            consumedByWall = wallGrabber.TryWallGrab(cachedHorizontal, cachedJumpPressed, cachedJumpHeld);
        }

        if (!consumedByWall)
        {
            jumper?.TryJump(cachedJumpPressed, cachedJumpHeld);
        }

        dasher?.TryDash(cachedDashPressed, cachedHorizontal);

        if (input != null)
        {
            if (cachedJumpPressed)
            {
                input.ConsumeJumpPressed();
                cachedJumpPressed = false;
            }
            if (cachedDashPressed)
            {
                input.ConsumeDashPressed();
                cachedDashPressed = false;
            }
        }

        if (digger != null)
        {
            digger.TryStartDig(input.DigHeld);
            digger.UpdateDig(Time.fixedDeltaTime, input.DigHeld);

            if (digger.IsDigging)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
