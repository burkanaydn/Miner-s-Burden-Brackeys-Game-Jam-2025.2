public interface IWallGrabAbility
{
    /// <summary>
    /// Tutunuyor mu (UI/animasyon veya diðer sistemler için).
    /// </summary>
    bool IsGrabbing { get; }

    /// <summary>
    /// Controller tarafýndan her FixedUpdate'te çaðrýlacak.
    /// horizontalInput: -1..1
    /// jumpPressed: o karede jump tuþuna basýldý mý (KeyDown)
    /// jumpHeld: jump tuþu basýlý mý
    /// Döndürülen bool: jumpPressed bu method tarafýndan tüketildi (ör. wall-jump yapýldý).
    /// </summary>
    bool TryWallGrab(float horizontalInput, bool jumpPressed, bool jumpHeld);
}
