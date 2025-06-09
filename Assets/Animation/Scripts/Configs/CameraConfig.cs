using UnityEngine;

namespace Animation.Scripts.Configs
{
    /// <summary>
    /// Конфигурация для камеры.
    /// </summary>
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Настройки камеры")]
        [Tooltip("Смещение камеры относительно игрока.")]
        public Vector3 cameraOffset = new(6.0f, 10.0f, -6.0f);
    }
}