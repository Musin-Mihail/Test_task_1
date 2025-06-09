using UnityEngine;

namespace Animation.Scripts.Configs
{
    /// <summary>
    /// Главный ScriptableObject, который теперь содержит ссылки на более мелкие конфигурации.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Компоненты Конфигурации")]
        public MovementConfig movementConfig;
        public FinisherConfig finisherConfig;
        public CameraConfig cameraConfig;
    }
}