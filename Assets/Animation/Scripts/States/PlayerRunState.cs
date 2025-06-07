using Animation.Scripts.Constants;
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
            Context.PlayerController.OnSpacePressed += OnSpacePressed;
            UpdateAndPlayMovementAnimation();
        }

        public override void ExitState()
        {
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;
        }

        public override void UpdateState()
        {
            if (!Context.PlayerMovement.IsMoving())
            {
                Context.ChangeState(Context.GetState<PlayerIdleState>());
                return;
            }

            UpdateAndPlayMovementAnimation();
        }

        public override void FixedUpdateState()
        {
            Context.PlayerMovement.Move();
        }

        public override void LateUpdateState()
        {
            Context.PlayerMovement.RotationToMouse();
        }

        /// <summary>
        /// Определяет доминирующее направление движения и воспроизводит соответствующую анимацию,
        /// получая данные напрямую из PlayerMovement.
        /// </summary>
        private void UpdateAndPlayMovementAnimation()
        {
            var isMovingForward = Context.PlayerMovement.IsMovingForward;
            var isMovingBack = Context.PlayerMovement.IsMovingBack;
            var isMovingLeft = Context.PlayerMovement.IsMovingLeft;
            var isMovingRight = Context.PlayerMovement.IsMovingRight;

            var animationName = "";

            if (isMovingForward && isMovingBack && isMovingLeft && isMovingRight)
            {
                animationName = PlayerAnimationNames.Idle;
            }
            else if (isMovingForward && isMovingRight || isMovingForward && isMovingLeft)
            {
                animationName = PlayerAnimationNames.RunRifle;
            }
            else if (isMovingBack && isMovingRight || isMovingBack && isMovingLeft)
            {
                animationName = PlayerAnimationNames.BackRunRifle;
            }
            else if (isMovingForward && !isMovingBack)
            {
                animationName = PlayerAnimationNames.RunRifle;
            }
            else if (isMovingBack && !isMovingForward)
            {
                animationName = PlayerAnimationNames.BackRunRifle;
            }
            else if (isMovingLeft && !isMovingRight)
            {
                animationName = PlayerAnimationNames.RunLeftRifle;
            }
            else if (isMovingRight && !isMovingLeft)
            {
                animationName = PlayerAnimationNames.RunRightRifle;
            }
            else
            {
                animationName = PlayerAnimationNames.Idle;
            }

            if (!string.IsNullOrEmpty(animationName))
            {
                Context.PlayerAnimation.PlayAnimation(animationName);
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
    }
}