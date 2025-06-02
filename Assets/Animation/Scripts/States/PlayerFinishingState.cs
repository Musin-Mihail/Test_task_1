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
            PlayerStateMachine.PlayerCombat.StartFinishing();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (!PlayerStateMachine.PlayerCombat.IsFinishing())
            {
                PlayerStateMachine.ChangeState(new PlayerIdleState(PlayerStateMachine));
            }
        }
    }
}