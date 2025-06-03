namespace Animation.Scripts.States
{
    public class PlayerRunState : PlayerState
    {
        public PlayerRunState(IPlayerStateContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Context.PlayerController.OnMoveForwardReleased += OnMoveReleased;
            Context.PlayerController.OnMoveBackReleased += OnMoveReleased;
            Context.PlayerController.OnMoveLeftReleased += OnMoveReleased;
            Context.PlayerController.OnMoveRightReleased += OnMoveReleased;

            Context.PlayerController.OnMoveForwardKeyDown += OnMoveKeyDown;
            Context.PlayerController.OnMoveBackKeyDown += OnMoveKeyDown;
            Context.PlayerController.OnMoveLeftKeyDown += OnMoveKeyDown;
            Context.PlayerController.OnMoveRightKeyDown += OnMoveKeyDown;

            Context.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            Context.PlayerController.OnMoveForwardReleased -= OnMoveReleased;
            Context.PlayerController.OnMoveBackReleased -= OnMoveReleased;
            Context.PlayerController.OnMoveLeftReleased -= OnMoveReleased;
            Context.PlayerController.OnMoveRightReleased -= OnMoveReleased;

            Context.PlayerController.OnMoveForwardKeyDown -= OnMoveKeyDown;
            Context.PlayerController.OnMoveBackKeyDown -= OnMoveKeyDown;
            Context.PlayerController.OnMoveLeftKeyDown -= OnMoveKeyDown;
            Context.PlayerController.OnMoveRightKeyDown -= OnMoveKeyDown;

            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        public override void FixedUpdateState()
        {
            Context.PlayerMovement.Move();
        }

        public override void LateUpdateState()
        {
            Context.PlayerMovement.RotationToMouse();
        }

        private void OnMoveReleased()
        {
            if (!Context.PlayerMovement.IsMoving())
            {
                Context.ChangeState(Context.GetIdleState());
            }
        }

        private void OnMoveKeyDown(string animationName)
        {
            Context.PlayerAnimation.PlayAnimation(animationName);
        }


        private void OnSpacePressed()
        {
            if (Context.EnemyFinishingTrigger.TryStartFinishing())
            {
                Context.ChangeState(Context.GetFinishingState());
            }
        }
    }
}