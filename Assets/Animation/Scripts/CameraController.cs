using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за позиционирование камеры относительно игрока.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;
        private readonly Vector3 _cameraOffset = new(6.0f, 10.0f, -6.0f);
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            if (!_camera)
            {
                enabled = false;
            }

            if (!playerTransform)
            {
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            if (playerTransform && _camera)
            {
                _camera.transform.position = playerTransform.position + _cameraOffset;
            }
        }
    }
}