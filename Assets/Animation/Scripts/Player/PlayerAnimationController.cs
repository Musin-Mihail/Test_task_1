using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику выбора и воспроизведения анимации движения игрока.
    /// Запрашивает данные о движении у PlayerMovement и передает команды в PlayerAnimation.
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour, IPlayerAnimationController
    {
        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerMovement">Ссылка на IPlayerMovement.</param>
        /// <param name="playerAnimation">Ссылка на IPlayerAnimation.</param>
        public void Initialize(IPlayerMovement playerMovement, IPlayerAnimation playerAnimation)
        {
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;
            if (_playerMovement == null)
            {
                Debug.LogError("PlayerMovement не был внедрен в PlayerAnimationController.");
            }

            if (_playerAnimation == null)
            {
                Debug.LogError("PlayerAnimation не был внедрен в PlayerAnimationController.");
            }
        }

        /// <summary>
        /// Определяет доминирующее направление движения и воспроизводит соответствующую анимацию,
        /// получая данные напрямую из PlayerMovement.
        /// </summary>
        public void UpdateAndPlayMovementAnimation()
        {
            if (_playerMovement == null || _playerAnimation == null)
            {
                return;
            }

            var isMovingForward = _playerMovement.IsMovingForward;
            var isMovingBack = _playerMovement.IsMovingBack;
            var isMovingLeft = _playerMovement.IsMovingLeft;
            var isMovingRight = _playerMovement.IsMovingRight;

            var animationName = "";

            if (_playerMovement.IsMoving())
            {
                if (isMovingForward && isMovingRight || isMovingForward && isMovingLeft)
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
            }
            else
            {
                animationName = PlayerAnimationNames.Idle;
            }


            if (!string.IsNullOrEmpty(animationName))
            {
                _playerAnimation.PlayAnimation(animationName);
            }
        }
    }
}