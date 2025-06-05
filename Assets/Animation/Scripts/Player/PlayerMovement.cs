using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        public Transform chest;

        private bool IsMovingForward { get; set; }
        private bool IsMovingBack { get; set; }
        private bool IsMovingLeft { get; set; }
        private bool IsMovingRight { get; set; }
        private PlayerController _playerController;
        private PlayerFinisher _playerFinisher;
        private const float Speed = 10f;
        private Camera _camera;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="controller">Ссылка на PlayerController.</param>
        /// <param name="finisher">Ссылка на PlayerFinisher.</param>
        public void Initialize(PlayerController controller, PlayerFinisher finisher)
        {
            _playerController = controller;
            _playerFinisher = finisher;
            _camera = Camera.main;

            if (_playerController)
            {
                SubscribeToControllerEvents();
            }
            else
            {
                Debug.LogError("PlayerController не был внедрен в PlayerMovement.");
            }
        }

        private void OnDisable()
        {
            if (_playerController)
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
            _playerController.OnMoveForwardPressed += OnMoveForwardPressedHandler;
            _playerController.OnMoveForwardReleased += OnMoveForwardReleasedHandler;
            _playerController.OnMoveBackPressed += OnMoveBackPressedHandler;
            _playerController.OnMoveBackReleased += OnMoveBackReleasedHandler;
            _playerController.OnMoveLeftPressed += OnMoveLeftPressedHandler;
            _playerController.OnMoveLeftReleased += OnMoveLeftReleasedHandler;
            _playerController.OnMoveRightPressed += OnMoveRightPressedHandler;
            _playerController.OnMoveRightReleased += OnMoveRightReleasedHandler;
        }

        private void UnsubscribeFromControllerEvents()
        {
            _playerController.OnMoveForwardPressed -= OnMoveForwardPressedHandler;
            _playerController.OnMoveForwardReleased -= OnMoveForwardReleasedHandler;
            _playerController.OnMoveBackPressed -= OnMoveBackPressedHandler;
            _playerController.OnMoveBackReleased -= OnMoveBackReleasedHandler;
            _playerController.OnMoveLeftPressed -= OnMoveLeftPressedHandler;
            _playerController.OnMoveLeftReleased -= OnMoveLeftReleasedHandler;
            _playerController.OnMoveRightPressed -= OnMoveRightPressedHandler;
            _playerController.OnMoveRightReleased -= OnMoveRightReleasedHandler;
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
            var mouseWorldPosition = _playerController.GetMouseWorldPosition();
            chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - chest.position);
            chest.transform.Rotate(-30, 90, 0);
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону цели.
        /// </summary>
        public void RotationToTarget()
        {
            chest.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - chest.position);
            chest.transform.Rotate(-30, 90, 10);
            transform.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - transform.position);
            transform.transform.Rotate(270, 180, 0);
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
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
        /// Перемещает игрока к указанной цели до достижения заданного расстояния.
        /// </summary>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <param name="stopDistance">Расстояние до цели, на котором нужно остановиться.</param>
        public IEnumerator MoveToTarget(Vector3 targetPosition, float stopDistance)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            while (distanceToTarget > stopDistance)
            {
                var directionToTarget = (targetPosition - transform.position).normalized;
                transform.position += directionToTarget * (Speed * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                yield return null;
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