using System;
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
    public class PlayerMovement : IPlayerMovement, IDisposable
    {
        private readonly PlayerConfig _playerConfig;
        private readonly PlayerMovementState _playerMovementState;
        private readonly Camera _camera;
        private readonly Transform _playerTransform;

        public Vector3 CurrentMovementInput => _playerMovementState.CurrentMovementInput;

        [Inject]
        public PlayerMovement(PlayerMovementState movementState, PlayerConfig config, [Inject(Id = "PlayerTransform")] Transform playerTransform)
        {
            _playerMovementState = movementState;
            _playerConfig = config;
            _playerTransform = playerTransform;

            _camera = Camera.main;
        }

        public void Dispose()
        {
            _playerMovementState?.Dispose();
        }

        public void Move()
        {
            if (!_camera || !_playerConfig) return;

            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRightFlat = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

            var moveDirection = cameraForwardFlat * CurrentMovementInput.z + cameraRightFlat * CurrentMovementInput.x;
            _playerTransform.position += moveDirection * (_playerConfig.speed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Проверяет, движется ли игрок в данный момент, используя PlayerMovementState.
        /// </summary>
        public bool IsMoving()
        {
            return _playerMovementState.IsMoving();
        }
    }
}