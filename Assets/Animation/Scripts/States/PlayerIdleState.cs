using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
// Добавлено для MovementDirection и KeyState

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
            Context.PlayerController.OnMovementIntent += OnMovementIntent;
            Context.PlayerController.OnSpacePressed += OnSpacePressed;
        }

        public override void ExitState()
        {
            Context.PlayerController.OnMovementIntent -= OnMovementIntent;
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        /// <summary>
        /// Обрабатывает абстрактное событие намерения движения.
        /// Если игрок начинает движение (клавиша нажата), переключается в состояние бега.
        /// </summary>
        /// <param name="direction">Направление движения.</param>
        /// <param name="state">Состояние клавиши (нажата/отпущена/удерживается).</param>
        private void OnMovementIntent(MovementDirection direction, KeyState state)
        {
            if (state == KeyState.Down)
            {
                Context.ChangeState(Context.GetState<PlayerRunState>());
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия пробела.
        /// </summary>
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