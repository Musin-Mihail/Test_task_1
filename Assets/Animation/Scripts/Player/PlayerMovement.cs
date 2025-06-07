using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// Теперь отделен от логики состояния движения и перемещения к цели.
    /// </summary>
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private PlayerConfig playerConfig;

        private IPlayerController _playerController;
        private IPlayerAnimationController _playerAnimationController;
        private PlayerMovementState _playerMovementState;
        private Camera _camera;

        public Vector3 CurrentMovementInput => _playerMovementState.CurrentMovementInput;

        public void Initialize(IPlayerController controller, IPlayerAnimationController animationController, PlayerMovementState movementState)
        {
            _playerController = controller;
            _playerAnimationController = animationController;
            _playerMovementState = movementState;
            _camera = Camera.main;

            if (!playerConfig)
            {
                Debug.LogError("PlayerConfig не назначен в инспекторе PlayerMovement. Пожалуйста, назначьте его.");
                enabled = false;
                return;
            }

            if (_playerController == null)
            {
                Debug.LogError("PlayerController не был внедрен в PlayerMovement.");
            }

            if (_playerAnimationController == null)
            {
                Debug.LogError("PlayerAnimationController не был внедрен в PlayerMovement.");
            }

            if (_playerMovementState == null)
            {
                Debug.LogError("PlayerMovementState не был внедрен в PlayerMovement.");
            }
        }

        private void OnDisable()
        {
            _playerMovementState?.Dispose();
        }

        public void Move()
        {
            if (!_camera) return;

            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRightFlat = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

            var moveDirection = cameraForwardFlat * CurrentMovementInput.z + cameraRightFlat * CurrentMovementInput.x;
            transform.position += moveDirection * (playerConfig.speed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Проверяет, движется ли игрок в данный момент, используя PlayerMovementState.
        /// </summary>
        public bool IsMoving()
        {
            return _playerMovementState.IsMoving();
        }

        public void UpdateMovementAndAnimation()
        {
            _playerAnimationController?.UpdateAndPlayMovementAnimation();
        }
    }
}