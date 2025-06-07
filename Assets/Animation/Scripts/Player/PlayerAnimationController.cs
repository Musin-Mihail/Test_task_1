using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику определения состояния движения игрока
    /// и установки соответствующих параметров в Animator.
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour, IPlayerAnimationController
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveZ = Animator.StringToHash("MoveZ");
        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;

        public float animationSmoothTime = 0.1f;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerMovement">Ссылка на IPlayerMovement.</param>
        /// <param name="playerAnimation">Ссылка на PlayerAnimation.</param>
        public void Initialize(IPlayerMovement playerMovement, IPlayerAnimation playerAnimation) // Изменили тип
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
        /// Обновляет параметры аниматора в зависимости от движения игрока.
        /// </summary>
        public void UpdateAndPlayMovementAnimation()
        {
            if (_playerMovement == null || _playerAnimation == null || !_playerAnimation.Animator)
            {
                return;
            }

            var currentMoveDirection = Vector3.zero;
            if (_playerMovement.IsMovingForward) currentMoveDirection += Vector3.forward;
            if (_playerMovement.IsMovingBack) currentMoveDirection += Vector3.back;
            if (_playerMovement.IsMovingLeft) currentMoveDirection += Vector3.left;
            if (_playerMovement.IsMovingRight) currentMoveDirection += Vector3.right;

            currentMoveDirection.Normalize();

            var targetMoveX = currentMoveDirection.x;
            var targetMoveZ = currentMoveDirection.z;

            var currentMoveX = _playerAnimation.Animator.GetFloat(MoveX);
            var currentMoveZ = _playerAnimation.Animator.GetFloat(MoveZ);

            currentMoveX = Mathf.Lerp(currentMoveX, targetMoveX, Time.deltaTime / animationSmoothTime);
            currentMoveZ = Mathf.Lerp(currentMoveZ, targetMoveZ, Time.deltaTime / animationSmoothTime);

            _playerAnimation.SetFloat("MoveX", currentMoveX);
            _playerAnimation.SetFloat("MoveZ", currentMoveZ);
            _playerAnimation.SetBool("IsMoving", _playerMovement.IsMoving());
        }
    }
}