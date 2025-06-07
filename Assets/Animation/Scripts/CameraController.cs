using Animation.Scripts.ScriptableObjects;
using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за позиционирование камеры относительно игрока.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public Transform playerTransform;
        [SerializeField] private PlayerConfig playerConfig;

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

            if (!playerConfig)
            {
                Debug.LogError("PlayerConfig не назначен в инспекторе CameraController. Пожалуйста, назначьте его.");
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            if (playerTransform && _camera && playerConfig)
            {
                _camera.transform.position = playerTransform.position + playerConfig.cameraOffset;
            }
        }
    }
}