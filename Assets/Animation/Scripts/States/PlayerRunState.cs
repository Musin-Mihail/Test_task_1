namespace Animation.Scripts.States
{
    public class PlayerRunState : PlayerState
    {
        public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void EnterState()
        {
            PlayerStateMachine.PlayerController.OnMoveForwardReleased += OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveBackReleased += OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveLeftReleased += OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveRightReleased += OnMoveReleased;

            PlayerStateMachine.PlayerController.OnMoveForwardKeyDown += OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveBackKeyDown += OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveLeftKeyDown += OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveRightKeyDown += OnMoveKeyDown;

            PlayerStateMachine.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            PlayerStateMachine.PlayerController.OnMoveForwardReleased -= OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveBackReleased -= OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveLeftReleased -= OnMoveReleased;
            PlayerStateMachine.PlayerController.OnMoveRightReleased -= OnMoveReleased;

            PlayerStateMachine.PlayerController.OnMoveForwardKeyDown -= OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveBackKeyDown -= OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveLeftKeyDown -= OnMoveKeyDown;
            PlayerStateMachine.PlayerController.OnMoveRightKeyDown -= OnMoveKeyDown;

            PlayerStateMachine.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        public override void FixedUpdateState()
        {
            PlayerStateMachine.PlayerMovement.Move();
        }

        public override void LateUpdateState()
        {
            PlayerStateMachine.PlayerMovement.RotationToMouse();
        }

        private void OnMoveReleased()
        {
            if (!PlayerStateMachine.PlayerMovement.IsMoving())
            {
                PlayerStateMachine.ChangeState(new PlayerIdleState(PlayerStateMachine));
            }
        }

        private void OnMoveKeyDown(string animationName)
        {
            PlayerStateMachine.PlayerAnimation.PlayAnimation(animationName);
        }


        private void OnSpacePressed()
        {
            if (PlayerStateMachine.EnemyFinishingTrigger.TryStartFinishing())
            {
                PlayerStateMachine.ChangeState(new PlayerFinishingState(PlayerStateMachine));
            }
        }
    }
}