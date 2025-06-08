using Animation.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Animation.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;
        private CameraConfig _cameraConfig;
        private Camera _camera;

        [Inject]
        public void Construct(PlayerConfig playerConfig)
        {
            _cameraConfig = playerConfig.cameraConfig;
        }

        private void Start()
        {
            _camera = Camera.main;
            if (!_camera || !playerTransform || !_cameraConfig)
            {
                enabled = false;
                Debug.LogError("CameraController не смог инициализироваться. Проверьте все зависимости.");
            }
        }

        private void LateUpdate()
        {
            if (playerTransform && _camera && _cameraConfig)
            {
                _camera.transform.position = playerTransform.position + _cameraConfig.cameraOffset;
            }
        }
    }
}