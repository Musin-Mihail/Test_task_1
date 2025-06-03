namespace Animation.Scripts.States
{
    public class PlayerFinishingState : PlayerState
    {
        public PlayerFinishingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            PlayerStateMachine.PlayerMovement.RotationToTarget();
            PlayerStateMachine.PlayerFinisher.StartFinishingSequence();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (!PlayerStateMachine.PlayerFinisher.IsFinishing())
            {
                PlayerStateMachine.ChangeState(new PlayerIdleState(PlayerStateMachine));
            }
        }
    }
}