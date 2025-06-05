using Animation.Scripts.States;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerStateContext
    {
        IPlayerAnimation PlayerAnimation { get; }
        IPlayerMovement PlayerMovement { get; }
        IPlayerController PlayerController { get; }
        IEnemyFinishingTrigger EnemyFinishingTrigger { get; }
        IPlayerFinisher PlayerFinisher { get; }
        void ChangeState(PlayerState newState);
        PlayerIdleState GetIdleState();
        PlayerRunState GetRunState();
        PlayerFinishingState GetFinishingState();
    }
}