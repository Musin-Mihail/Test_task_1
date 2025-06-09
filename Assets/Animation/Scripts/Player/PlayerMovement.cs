using Animation.Scripts.Configs;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public interface IPlayerMovement
    {
        void Move(Vector3 direction);
    }

    public class PlayerMovement : IPlayerMovement
    {
        private readonly Transform _playerTransform;
        private readonly MovementConfig _movementConfig;
        private readonly Camera _camera;

        public PlayerMovement(Transform playerTransform, MovementConfig movementConfig, Camera camera)
        {
            _playerTransform = playerTransform;
            _movementConfig = movementConfig;
            _camera = camera;
        }

        public void Move(Vector3 moveInput)
        {
            var cameraForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var cameraRight = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;
            var moveDirection = (cameraForward * moveInput.z + cameraRight * moveInput.x).normalized;
            _playerTransform.position += moveDirection * (_movementConfig.speed * Time.fixedDeltaTime);
        }
    }
}