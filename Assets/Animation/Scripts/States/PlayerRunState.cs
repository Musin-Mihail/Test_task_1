using Animation.Scripts.Interfaces;

namespace Animation.Scripts.States
{
    public class PlayerRunState : PlayerState
    {
        public PlayerRunState(IPlayerStateContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Context.PlayerAnimation.SetBool("IsMoving", true);
            Context.PlayerController.OnSpacePressed += OnSpacePressed;
            Context.PlayerMovement.UpdateMovementAndAnimation();
        }

        public override void ExitState()
        {
            Context.PlayerAnimation.SetBool("IsMoving", false);
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        public override void UpdateState()
        {
            if (!Context.PlayerMovement.IsMoving())
            {
                Context.ChangeState<PlayerIdleState>();
                return;
            }

            Context.PlayerMovement.UpdateMovementAndAnimation();
        }

        public override void FixedUpdateState()
        {
            Context.PlayerMovement.Move();
        }

        public override void LateUpdateState()
        {
            Context.PlayerRotator.RotationToMouse();
        }

        /// <summary>
        /// Обрабатывает событие нажатия пробела.
        /// </summary>
        private void OnSpacePressed()
        {
            if (Context.EnemyFinishingTrigger.TryStartFinishing())
            {
                Context.ChangeState<PlayerFinishingState>();
            }
        }
    }
}