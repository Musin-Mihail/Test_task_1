using Zenject;

namespace Animation.Scripts.States
{
    public class PlayerIdleState : PlayerState
    {
        [Inject]
        public PlayerIdleState()
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