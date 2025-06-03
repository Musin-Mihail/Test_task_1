namespace Animation.Scripts.States
{
    public interface IPlayerStateContext
    {
        PlayerAnimation PlayerAnimation { get; }
        PlayerMovement PlayerMovement { get; }
        PlayerController PlayerController { get; }
        EnemyFinishingTrigger EnemyFinishingTrigger { get; }
        PlayerFinisher PlayerFinisher { get; }
        void ChangeState(PlayerState newState);
        PlayerIdleState GetIdleState();
        PlayerRunState GetRunState();
        PlayerFinishingState GetFinishingState();
    }
}