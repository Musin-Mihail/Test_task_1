using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public interface IPlayerRotator
    {
        void RotateToMouse();
        void RotateToTarget(Vector3 targetPosition);
        void RotateTowardsCamera();
    }

    public class PlayerRotator : IPlayerRotator
    {
        private readonly Transform _playerTransform;
        private readonly Transform _chestTransform;
        private readonly IPlayerInput _playerInput;
        private readonly Camera _camera;

        public PlayerRotator(Transform playerTransform, [Inject(Id = "ChestTransform")] Transform chestTransform, IPlayerInput playerInput)
        {
            _playerTransform = playerTransform;
            _chestTransform = chestTransform;
            _playerInput = playerInput;
            _camera = Camera.main;
        }

        public void RotateToMouse()
        {
            var mouseWorldPosition = _playerInput.GetMouseWorldPosition();
            _chestTransform.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - _chestTransform.position);
            _chestTransform.transform.Rotate(-30, 90, 0);
        }

        public void RotateToTarget(Vector3 targetPosition)
        {
            // Логика поворота к цели
            var playerLookDir = new Vector3(targetPosition.x, _playerTransform.position.y, targetPosition.z) - _playerTransform.position;
            if (playerLookDir != Vector3.zero)
            {
                _playerTransform.rotation = Quaternion.LookRotation(playerLookDir);
            }
        }

        public void RotateTowardsCamera()
        {
            var cameraForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            if (cameraForward != Vector3.zero)
            {
                _playerTransform.rotation = Quaternion.LookRotation(cameraForward);
            }
        }
    }
}