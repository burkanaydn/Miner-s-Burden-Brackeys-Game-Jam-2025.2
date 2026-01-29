using UnityEngine;

public class KeyboardInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private KeyCode jumpKey1 = KeyCode.Space;
    [SerializeField] private KeyCode jumpKey2 = KeyCode.UpArrow;
    [SerializeField] private KeyCode dashKey = KeyCode.X;
    [SerializeField] private KeyCode digKey = KeyCode.Z;

    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool DigHeld { get; private set; }
    public bool DashPressed { get; private set; }

    void Update()
    {
        float left = Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f;
        float right = Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;

        Horizontal = Mathf.Clamp(left + right, -1f, 1f);

        bool nowHeld = Input.GetKey(jumpKey1) || Input.GetKey(jumpKey2);
        JumpPressed |= Input.GetKeyDown(jumpKey1) || Input.GetKeyDown(jumpKey2);
        JumpHeld = nowHeld;

        DigHeld = Input.GetKey(digKey);

        DashPressed |= Input.GetKeyDown(dashKey);
    }

    public void ConsumeJumpPressed()
    {
        JumpPressed = false;
    }

    public void ConsumeDashPressed()
    {
        DashPressed = false;
    }
}
