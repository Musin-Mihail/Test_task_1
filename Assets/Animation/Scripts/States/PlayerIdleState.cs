namespace Animation.Scripts.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(IPlayerStateContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Context.PlayerAnimation.PlayAnimation(PlayerAnimationNames.Idle);
            Context.PlayerController.OnMoveForwardPressed += OnMovePressed;
            Context.PlayerController.OnMoveBackPressed += OnMovePressed;
            Context.PlayerController.OnMoveLeftPressed += OnMovePressed;
            Context.PlayerController.OnMoveRightPressed += OnMovePressed;
            Context.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            Context.PlayerController.OnMoveForwardPressed -= OnMovePressed;
            Context.PlayerController.OnMoveBackPressed -= OnMovePressed;
            Context.PlayerController.OnMoveLeftPressed -= OnMovePressed;
            Context.PlayerController.OnMoveRightPressed -= OnMovePressed;
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        private void OnMovePressed()
        {
            Context.ChangeState(Context.GetRunState());
        }

        private void OnSpacePressed()
        {
            if (Context.EnemyFinishingTrigger.TryStartFinishing())
            {
                Context.ChangeState(Context.GetFinishingState());
            }
        }

        public override void LateUpdateState()
        {
            Context.PlayerMovement.RotationToMouse();
        }
    }
}