using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику вращения игрока.
    /// </summary>
    public class PlayerRotator : MonoBehaviour, IPlayerRotator
    {
        [SerializeField] private Transform chest;

        private IPlayerController _playerController;
        private IPlayerFinisher _playerFinisher;
        private Camera _camera;

        [Inject]
        public void Construct(IPlayerController playerController, IPlayerFinisher playerFinisher)
        {
            _playerController = playerController;
            _playerFinisher = playerFinisher;
            _camera = Camera.main;
        }

        public void RotationToMouse()
        {
            if (_playerController == null || !chest) return;

            var mouseWorldPosition = _playerController.GetMouseWorldPosition();
            chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - chest.position);
            chest.transform.Rotate(-30, 90, 0);
        }

        public void RotationToTarget()
        {
            if (_playerFinisher == null || !chest) return;

            chest.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - chest.position);
            chest.transform.Rotate(-30, 90, 10);
            transform.rotation = Quaternion.LookRotation(Vector3.up, _playerFinisher.TargetPosition - transform.position);
            transform.transform.Rotate(270, 180, 0);
        }

        public void RotateTowardsCamera()
        {
            if (!_camera) return;

            var cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            if (cameraForwardFlat != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(cameraForwardFlat);
            }
        }
    }
}