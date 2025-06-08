using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику вращения игрока.
    /// </summary>
    public class PlayerRotator : IPlayerRotator
    {
        private readonly Transform _chest;
        private readonly IPlayerController _playerController;
        private readonly Camera _camera;
        private readonly Transform _playerTransform;

        [Inject]
        public PlayerRotator(
            IPlayerController playerController,
            [Inject(Id = "PlayerChestTransform")] Transform chest,
            [Inject(Id = "PlayerTransform")] Transform playerTransform
        )
        {
            _playerController = playerController;
            _chest = chest;
            _playerTransform = playerTransform;

            _camera = Camera.main;
        }

        [Inject]
        public void PostConstruct(IPlayerFinisher playerFinisher)
        {
            playerFinisher.OnFinisherStateReset += RotateTowardsCamera;
        }

        public void RotationToMouse()
        {
            if (_playerController == null || !_chest) return;

            var mouseWorldPosition = _playerController.GetMouseWorldPosition();
            _chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - _chest.position);
            _chest.transform.Rotate(-30, 90, 0);
        }

        public void RotationToTarget(Vector3 targetPosition)
        {
            if (!_chest) return;
            _chest.rotation = Quaternion.LookRotation(Vector3.up, targetPosition - _chest.position);
            _chest.transform.Rotate(-30, 90, 10);
            _playerTransform.rotation = Quaternion.LookRotation(Vector3.up, targetPosition - _playerTransform.position);
            _playerTransform.transform.Rotate(270, 180, 0);
        }

        public void RotateTowardsCamera()
        {
            if (!_camera) return;

            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            if (cameraForwardFlat != Vector3.zero)
            {
                _playerTransform.rotation = Quaternion.LookRotation(cameraForwardFlat);
            }
        }
    }
}