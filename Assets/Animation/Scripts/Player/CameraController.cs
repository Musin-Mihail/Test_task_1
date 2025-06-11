using Animation.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class CameraController : MonoBehaviour
    {
        private Transform _playerTransform;
        private CameraConfig _cameraConfig;
        private Camera _camera;

        [Inject]
        public void Construct(Transform playerTransform, CameraConfig cameraConfig, Camera camera)
        {
            _playerTransform = playerTransform;
            _cameraConfig = cameraConfig;
            _camera = camera;
        }

        private void LateUpdate()
        {
            if (_playerTransform && _camera)
            {
                var targetPosition = _playerTransform.position + _cameraConfig.cameraOffset;
                _camera.transform.position = targetPosition;
            }
        }
    }
}