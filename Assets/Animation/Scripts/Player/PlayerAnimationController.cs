using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;
// Добавляем using для доступа к PlayerConfig

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

        [SerializeField] private PlayerConfig playerConfig;

        private IPlayerMovement _playerMovement;
        private IPlayerAnimation _playerAnimation;

        public void Initialize(IPlayerMovement playerMovement, IPlayerAnimation playerAnimation)
        {
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;

            if (!playerConfig)
            {
                Debug.LogError("PlayerConfig не назначен в инспекторе PlayerAnimationController. Пожалуйста, назначьте его.");
                enabled = false;
                return;
            }

            if (_playerMovement == null)
            {
                Debug.LogError("PlayerMovement не был внедрен в PlayerAnimationController.");
            }

            if (_playerAnimation == null)
            {
                Debug.LogError("PlayerAnimation не был внедрен в PlayerAnimationController.");
            }
        }

        public void UpdateAndPlayMovementAnimation()
        {
            if (_playerMovement == null || _playerAnimation == null || !_playerAnimation.Animator || !playerConfig)
            {
                return;
            }

            var targetMoveDirection = _playerMovement.CurrentMovementInput;

            var targetMoveX = targetMoveDirection.x;
            var targetMoveZ = targetMoveDirection.z;

            var currentMoveX = _playerAnimation.Animator.GetFloat(MoveX);
            var currentMoveZ = _playerAnimation.Animator.GetFloat(MoveZ);

            currentMoveX = Mathf.Lerp(currentMoveX, targetMoveX, Time.deltaTime / playerConfig.animationSmoothTime);
            currentMoveZ = Mathf.Lerp(currentMoveZ, targetMoveZ, Time.deltaTime / playerConfig.animationSmoothTime);

            _playerAnimation.SetFloat("MoveX", currentMoveX);
            _playerAnimation.SetFloat("MoveZ", currentMoveZ);
            _playerAnimation.SetBool("IsMoving", _playerMovement.IsMoving());
        }
    }
}