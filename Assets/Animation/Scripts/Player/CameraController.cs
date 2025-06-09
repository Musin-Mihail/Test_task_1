using Animation.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public interface ICameraController
    {
    }

    public class CameraController : MonoBehaviour, ICameraController
    {
        private Transform _playerTransform;
        private CameraConfig _cameraConfig;
        private Camera _camera;

        [Inject]
        public void Construct(Transform playerTransform, CameraConfig cameraConfig)
        {
            _playerTransform = playerTransform;
            _cameraConfig = cameraConfig;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_playerTransform && _camera)
            {
                _camera.transform.position = _playerTransform.position + _cameraConfig.cameraOffset;
            }
        }
    }
}