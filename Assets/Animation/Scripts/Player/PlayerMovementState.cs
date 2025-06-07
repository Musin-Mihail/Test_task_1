using System;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за инкапсуляцию состояния движения игрока на основе ввода.
    /// Предоставляет чистое направление движения (Vector3).
    /// </summary>
    public class PlayerMovementState
    {
        private readonly IPlayerController _playerController;
        private Vector3 _currentMovementInput;

        public Vector3 CurrentMovementInput => _currentMovementInput;

        public PlayerMovementState(IPlayerController playerController)
        {
            _playerController = playerController;
            SubscribeToControllerEvents();
        }

        /// <summary>
        /// Подписывается на события контроллера игрока при создании.
        /// </summary>
        private void SubscribeToControllerEvents()
        {
            _playerController.OnMovementIntent += OnMovementIntentHandler;
        }

        /// <summary>
        /// Отписывается от событий контроллера игрока при уничтожении.
        /// </summary>
        public void Dispose()
        {
            _playerController.OnMovementIntent -= OnMovementIntentHandler;
        }

        /// <summary>
        /// Обработчик абстрактного события намерения движения.
        /// Обновляет текущее направление движения.
        /// </summary>
        /// <param name="direction">Направление движения.</param>
        /// <param name="state">Состояние клавиши (нажата/отпущена/удерживается).</param>
        private void OnMovementIntentHandler(MovementDirection direction, KeyState state)
        {
            var isPressed = state is KeyState.Pressed or KeyState.Down;

            switch (direction)
            {
                case MovementDirection.Forward:
                    _currentMovementInput.z = isPressed ? 1f : 0f;
                    break;
                case MovementDirection.Back:
                    _currentMovementInput.z = isPressed ? -1f : 0f;
                    break;
                case MovementDirection.Left:
                    _currentMovementInput.x = isPressed ? -1f : 0f;
                    break;
                case MovementDirection.Right:
                    _currentMovementInput.x = isPressed ? 1f : 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            if (_currentMovementInput.magnitude > 1f)
            {
                _currentMovementInput.Normalize();
            }
        }

        /// <summary>
        /// Проверяет, движется ли игрок в данный момент.
        /// </summary>
        public bool IsMoving()
        {
            return _currentMovementInput.magnitude > 0.01f;
        }
    }
}