using System;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за инкапсуляцию состояния движения игрока на основе ввода.
    /// Предоставляет чистое направление движения (Vector3).
    /// </summary>
    public class PlayerMovementState : IDisposable
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
            _playerController.OnMovementInput += OnMovementInputHandler;
        }

        /// <summary>
        /// Отписывается от событий контроллера игрока при уничтожении.
        /// </summary>
        public void Dispose()
        {
            _playerController.OnMovementInput -= OnMovementInputHandler;
        }

        /// <summary>
        /// Обработчик события ввода движения.
        /// Обновляет текущее направление движения.
        /// </summary>
        /// <param name="movementInput">Вектор2 движения, полученный из Input System.</param>
        private void OnMovementInputHandler(Vector2 movementInput)
        {
            _currentMovementInput.x = movementInput.x;
            _currentMovementInput.z = movementInput.y;
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