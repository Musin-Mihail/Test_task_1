using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// Теперь отделен от логики состояния движения и перемещения к цели.
    /// </summary>
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        private PlayerConfig _playerConfig;
        private IPlayerAnimationController _playerAnimationController;
        private PlayerMovementState _playerMovementState;
        private Camera _camera;

        public Vector3 CurrentMovementInput => _playerMovementState.CurrentMovementInput;


        [Inject]
        public void Construct(IPlayerAnimationController animationController, PlayerMovementState movementState, PlayerConfig config)
        {
            _playerAnimationController = animationController;
            _playerMovementState = movementState;
            _playerConfig = config;

            _camera = Camera.main;
        }

        private void OnDisable()
        {
            _playerMovementState?.Dispose();
        }

        public void Move()
        {
            if (!_camera || !_playerConfig) return;

            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRightFlat = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

            var moveDirection = cameraForwardFlat * CurrentMovementInput.z + cameraRightFlat * CurrentMovementInput.x;
            transform.position += moveDirection * (_playerConfig.speed * Time.fixedDeltaTime);
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