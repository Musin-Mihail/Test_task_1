using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// Реализует интерфейс IPlayerMovement.
    /// </summary>
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private Transform chest;
        private bool _isMovingForward;
        private bool _isMovingBack;
        private bool _isMovingLeft;
        private bool _isMovingRight;

        private IPlayerController _playerController;
        private IPlayerFinisher _playerFinisher;
        private const float Speed = 10f;
        private Camera _camera;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="controller">Ссылка на PlayerController.</param>
        /// <param name="finisher">Ссылка на PlayerFinisher.</param>
        public void Initialize(IPlayerController controller, IPlayerFinisher finisher)
        {
            _playerController = controller;
            _playerFinisher = finisher;
            _camera = Camera.main;

            if (_playerController != null)
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
            if (_playerController != null)
            {
                UnsubscribeFromControllerEvents();
            }
        }

        /// <summary>
        /// Подписывается на события контроллера игрока.
        /// </summary>
        private void SubscribeToControllerEvents()
        {
            _playerController.OnMovementIntent += OnMovementIntentHandler;
        }

        /// <summary>
        /// Отписывается от событий контроллера игрока.
        /// </summary>
        private void UnsubscribeFromControllerEvents()
        {
            _playerController.OnMovementIntent -= OnMovementIntentHandler;
        }

        /// <summary>
        /// Обработчик абстрактного события намерения движения.
        /// Обновляет флаги движения игрока на основе направления и состояния клавиши.
        /// </summary>
        /// <param name="direction">Направление движения.</param>
        /// <param name="state">Состояние клавиши (нажата/отпущена/удерживается).</param>
        private void OnMovementIntentHandler(MovementDirection direction, KeyState state)
        {
            var isPressedOrDown = state is KeyState.Pressed or KeyState.Down;

            switch (direction)
            {
                case MovementDirection.Forward:
                    _isMovingForward = isPressedOrDown;
                    break;
                case MovementDirection.Back:
                    _isMovingBack = isPressedOrDown;
                    break;
                case MovementDirection.Left:
                    _isMovingLeft = isPressedOrDown;
                    break;
                case MovementDirection.Right:
                    _isMovingRight = isPressedOrDown;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            if (state == KeyState.Released)
            {
                switch (direction)
                {
                    case MovementDirection.Forward:
                        _isMovingForward = false;
                        break;
                    case MovementDirection.Back:
                        _isMovingBack = false;
                        break;
                    case MovementDirection.Left:
                        _isMovingLeft = false;
                        break;
                    case MovementDirection.Right:
                        _isMovingRight = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Выполняет движение игрока. Вызывается из состояния FSM.
        /// </summary>
        public void Move()
        {
            if (!_camera) return;
            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRightFlat = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

            if (_isMovingForward)
            {
                transform.position += cameraForwardFlat * (Speed * Time.fixedDeltaTime);
            }

            if (_isMovingBack)
            {
                transform.position += -cameraForwardFlat * (Speed * Time.fixedDeltaTime);
            }

            if (_isMovingLeft)
            {
                transform.position += -cameraRightFlat * (Speed * Time.fixedDeltaTime);
            }

            if (_isMovingRight)
            {
                transform.position += cameraRightFlat * (Speed * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону указателя мыши.
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
            return _isMovingForward || _isMovingBack || _isMovingLeft || _isMovingRight;
        }
    }
}