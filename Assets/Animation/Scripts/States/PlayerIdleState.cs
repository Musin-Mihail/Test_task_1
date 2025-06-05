using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;

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
            Context.ChangeState(Context.GetState<PlayerRunState>());
        }

        private void OnSpacePressed()
        {
            if (Context.EnemyFinishingTrigger.TryStartFinishing())
            {
                Context.ChangeState(Context.GetState<PlayerFinishingState>());
            }
        }

        public override void LateUpdateState()
        {
            Context.PlayerMovement.RotationToMouse();
        }
    }
}