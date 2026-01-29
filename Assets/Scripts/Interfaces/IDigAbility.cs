public interface IDigAbility
{
    bool IsDigging { get; }
    void TryStartDig(bool digHeld);
    void UpdateDig(float deltaTime, bool digHeld);
}
