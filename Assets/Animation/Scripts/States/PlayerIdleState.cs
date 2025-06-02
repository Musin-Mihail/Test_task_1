namespace Animation.Scripts.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            PlayerStateMachine.PlayerAnimation.PlayAnimation("Idle");
            PlayerStateMachine.PlayerController.OnMoveForwardPressed += OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveBackPressed += OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveLeftPressed += OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveRightPressed += OnMovePressed;
            PlayerStateMachine.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            PlayerStateMachine.PlayerController.OnMoveForwardPressed -= OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveBackPressed -= OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveLeftPressed -= OnMovePressed;
            PlayerStateMachine.PlayerController.OnMoveRightPressed -= OnMovePressed;
            PlayerStateMachine.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        private void OnMovePressed()
        {
            PlayerStateMachine.ChangeState(new PlayerRunState(PlayerStateMachine));
        }

        private void OnSpacePressed()
        {
            if (!PlayerStateMachine.PlayerCombat.IsFinishing() && PlayerStateMachine.PlayerCombat.finishingText.activeSelf)
            {
                PlayerStateMachine.ChangeState(new PlayerFinishingState(PlayerStateMachine));
            }
        }

        public override void LateUpdateState()
        {
            PlayerStateMachine.PlayerMovement.RotationToMouse();
        }
    }
}