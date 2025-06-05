using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;

namespace Animation.Scripts.States
{
    public class PlayerRunState : PlayerState
    {
        private bool _isMovingForward;
        private bool _isMovingBack;
        private bool _isMovingLeft;
        private bool _isMovingRight;

        public PlayerRunState(IPlayerStateContext context) : base(context)
        {
        }

        public override void EnterState()
        {
            Context.PlayerController.OnMovementIntent += OnMovementIntent;
            Context.PlayerController.OnSpacePressed += OnSpacePressed;

            UpdateAndPlayMovementAnimation();
        }

        public override void ExitState()
        {
            Context.PlayerController.OnMovementIntent -= OnMovementIntent;
            Context.PlayerController.OnSpacePressed -= OnSpacePressed;

            _isMovingForward = false;
            _isMovingBack = false;
            _isMovingLeft = false;
            _isMovingRight = false;
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
        /// Обработчик абстрактного события намерения движения.
        /// Обновляет внутренние флаги движения и затем определяет, какую анимацию воспроизвести.
        /// </summary>
        /// <param name="direction">Направление движения.</param>
        /// <param name="state">Состояние клавиши (нажата/отпущена/удерживается).</param>
        private void OnMovementIntent(MovementDirection direction, KeyState state)
        {
            switch (direction)
            {
                case MovementDirection.Forward:
                    _isMovingForward = state is KeyState.Down or KeyState.Pressed;
                    break;
                case MovementDirection.Back:
                    _isMovingBack = state is KeyState.Down or KeyState.Pressed;
                    break;
                case MovementDirection.Left:
                    _isMovingLeft = state is KeyState.Down or KeyState.Pressed;
                    break;
                case MovementDirection.Right:
                    _isMovingRight = state is KeyState.Down or KeyState.Pressed;
                    break;
            }

            UpdateAndPlayMovementAnimation();

            if (state == KeyState.Released && !Context.PlayerMovement.IsMoving())
            {
                Context.ChangeState(Context.GetState<PlayerIdleState>());
            }
        }

        /// <summary>
        /// Определяет доминирующее направление движения и воспроизводит соответствующую анимацию.
        /// Приоритет отдается комбинациям (диагоналям), затем основным направлениям.
        /// </summary>
        private void UpdateAndPlayMovementAnimation()
        {
            var animationName = "";

            if (_isMovingForward && _isMovingBack && _isMovingLeft && _isMovingRight)
            {
                animationName = PlayerAnimationNames.Idle;
            }
            else if (_isMovingForward && _isMovingRight || _isMovingForward && _isMovingLeft)
            {
                animationName = PlayerAnimationNames.RunRifle;
            }
            else if (_isMovingBack && _isMovingRight || _isMovingBack && _isMovingLeft)
            {
                animationName = PlayerAnimationNames.BackRunRifle;
            }
            else if (_isMovingForward && !_isMovingBack)
            {
                animationName = PlayerAnimationNames.RunRifle;
            }
            else if (_isMovingBack && !_isMovingForward)
            {
                animationName = PlayerAnimationNames.BackRunRifle;
            }
            else if (_isMovingLeft && !_isMovingRight)
            {
                animationName = PlayerAnimationNames.RunLeftRifle;
            }
            else if (_isMovingRight && !_isMovingLeft)
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