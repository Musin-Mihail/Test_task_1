using Animation.Scripts.Interfaces;

namespace Animation.Scripts.States
{
    public class PlayerFinishingState : PlayerState
    {
        public PlayerFinishingState(IPlayerStateContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Context.PlayerMovement.RotationToTarget();
            Context.PlayerFinisher.StartFinishingSequence();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (!Context.PlayerFinisher.IsFinishing())
            {
                Context.ChangeState(Context.GetState<PlayerIdleState>());
            }
        }
    }
}