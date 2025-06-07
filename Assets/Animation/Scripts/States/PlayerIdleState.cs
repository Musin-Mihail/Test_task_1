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
            Context.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        public override void UpdateState()
        {
            if (Context.PlayerMovement.IsMoving())
            {
                Context.ChangeState<PlayerRunState>();
            }
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

        public override void LateUpdateState()
        {
            Context.PlayerRotator.RotationToMouse();
        }
    }
}