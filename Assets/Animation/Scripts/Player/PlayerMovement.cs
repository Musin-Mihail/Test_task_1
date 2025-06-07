using Animation.Scripts.Interfaces;
using UnityEngine;

// Убедитесь, что это добавлено

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// Теперь отделен от логики состояния движения и перемещения к цели.
    /// </summary>
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private Transform chest;
        [SerializeField] private float speed = 10f;

        private IPlayerController _playerController;
        private IPlayerFinisher _playerFinisher;
        private IPlayerAnimationController _playerAnimationController;
        private PlayerMovementState _playerMovementState;
        private Camera _camera;

        public Vector3 CurrentMovementInput => _playerMovementState.CurrentMovementInput;

        public void Initialize(IPlayerController controller, IPlayerFinisher finisher, IPlayerAnimationController animationController, PlayerMovementState movementState)
        {
            _playerController = controller;
            _playerFinisher = finisher;
            _playerAnimationController = animationController;
            _playerMovementState = movementState;
            _camera = Camera.main;

            if (_playerController == null)
            {
                Debug.LogError("PlayerController не был внедрен в PlayerMovement.");
            }

            if (_playerFinisher == null)
            {
                Debug.LogError("PlayerFinisher не был внедрен в PlayerMovement.");
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
            transform.position += moveDirection * (speed * Time.fixedDeltaTime);
        }

        public void RotationToMouse()
        {
            var mouseWorldPosition = _playerController.GetMouseWorldPosition();
            chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - chest.position);
            chest.transform.Rotate(-30, 90, 0);
        }

        public void RotationToTarget()
        {
            chest.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - chest.position);
            chest.transform.Rotate(-30, 90, 10);
            transform.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - transform.position);
            transform.transform.Rotate(270, 180, 0);
        }

        public void RotateTowardsCamera()
        {
            if (_camera)
            {
                var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                if (cameraForwardFlat != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(cameraForwardFlat);
                }
            }
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