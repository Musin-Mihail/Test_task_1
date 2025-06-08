using UnityEngine;

namespace Animation.Scripts.ScriptableObjects
{
    /// <summary>
    /// Главный ScriptableObject, который теперь содержит ссылки на более мелкие конфигурации.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Компоненты Конфигурации")]
        public MovementConfig movementConfig;
        public FinisherConfig finisherConfig;
        public CameraConfig cameraConfig;
    }
}