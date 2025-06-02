using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        private bool IsMovingForward { get; set; }
        private bool IsMovingBack { get; set; }
        private bool IsMovingLeft { get; set; }
        private bool IsMovingRight { get; set; }
        public Transform chest;

        private const float Speed = 10f;
        private PlayerStateMachine _playerStateMachine;
        private Camera _camera;

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _camera = Camera.main;

            if (!_playerStateMachine)
            {
                Debug.LogError("PlayerStateMachine не найден на объекте PlayerMovement.");
                enabled = false;
            }

            if (_playerStateMachine.PlayerController)
            {
                SubscribeToControllerEvents();
            }
        }

        private void OnDisable()
        {
            if (_playerStateMachine.PlayerController)
            {
                UnsubscribeFromControllerEvents();
            }
        }

        private void OnMoveForwardPressedHandler() => IsMovingForward = true;
        private void OnMoveForwardReleasedHandler() => IsMovingForward = false;
        private void OnMoveBackPressedHandler() => IsMovingBack = true;
        private void OnMoveBackReleasedHandler() => IsMovingBack = false;
        private void OnMoveLeftPressedHandler() => IsMovingLeft = true;
        private void OnMoveLeftReleasedHandler() => IsMovingLeft = false;
        private void OnMoveRightPressedHandler() => IsMovingRight = true;
        private void OnMoveRightReleasedHandler() => IsMovingRight = false;

        private void SubscribeToControllerEvents()
        {
            _playerStateMachine.PlayerController.OnMoveForwardPressed += OnMoveForwardPressedHandler;
            _playerStateMachine.PlayerController.OnMoveForwardReleased += OnMoveForwardReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveBackPressed += OnMoveBackPressedHandler;
            _playerStateMachine.PlayerController.OnMoveBackReleased += OnMoveBackReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveLeftPressed += OnMoveLeftPressedHandler;
            _playerStateMachine.PlayerController.OnMoveLeftReleased += OnMoveLeftReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveRightPressed += OnMoveRightPressedHandler;
            _playerStateMachine.PlayerController.OnMoveRightReleased += OnMoveRightReleasedHandler;
        }

        private void UnsubscribeFromControllerEvents()
        {
            _playerStateMachine.PlayerController.OnMoveForwardPressed -= OnMoveForwardPressedHandler;
            _playerStateMachine.PlayerController.OnMoveForwardReleased -= OnMoveForwardReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveBackPressed -= OnMoveBackPressedHandler;
            _playerStateMachine.PlayerController.OnMoveBackReleased -= OnMoveBackReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveLeftPressed -= OnMoveLeftPressedHandler;
            _playerStateMachine.PlayerController.OnMoveLeftReleased -= OnMoveLeftReleasedHandler;
            _playerStateMachine.PlayerController.OnMoveRightPressed -= OnMoveRightPressedHandler;
            _playerStateMachine.PlayerController.OnMoveRightReleased -= OnMoveRightReleasedHandler;
        }

        /// <summary>
        /// Выполняет движение игрока. Вызывается из состояния FSM.
        /// </summary>
        public void Move()
        {
            if (!_camera) return;
            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRightFlat = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

            if (IsMovingForward)
            {
                transform.position += cameraForwardFlat * (Speed * Time.fixedDeltaTime);
            }

            if (IsMovingBack)
            {
                transform.position += -cameraForwardFlat * (Speed * Time.fixedDeltaTime);
            }

            if (IsMovingLeft)
            {
                transform.position += -cameraRightFlat * (Speed * Time.fixedDeltaTime);
            }

            if (IsMovingRight)
            {
                transform.position += cameraRightFlat * (Speed * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону указателя мышки.
        /// </summary>
        public void RotationToMouse()
        {
            var mouseWorldPosition = _playerStateMachine.PlayerController.GetMouseWorldPosition();
            chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - chest.position);
            chest.transform.Rotate(-30, 90, 0);
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону цели.
        /// </summary>
        public void RotationToTarget()
        {
            chest.rotation = Quaternion.LookRotation(Vector3.up, _playerStateMachine.PlayerCombat.CurrentTarget - chest.position);
            chest.transform.Rotate(-30, 90, 10);
            transform.rotation = Quaternion.LookRotation(Vector3.up, _playerStateMachine.PlayerCombat.CurrentTarget - transform.position);
            transform.transform.Rotate(270, 180, 0);
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
        public void RotateTowardsCamera()
        {
            if (_camera)
            {
                Vector3 cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                if (cameraForwardFlat != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(cameraForwardFlat);
                }
            }
        }

        /// <summary>
        /// Возвращает, движется ли игрок в данный момент.
        /// </summary>
        public bool IsMoving()
        {
            return IsMovingForward || IsMovingBack || IsMovingLeft || IsMovingRight;
        }
    }
}