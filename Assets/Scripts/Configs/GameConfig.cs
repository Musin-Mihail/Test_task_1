using UnityEngine;

namespace Animation.Scripts.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Компоненты Конфигурации")]
        public MovementConfig movementConfig;
        public FinisherConfig finisherConfig;
        public CameraConfig cameraConfig;
        public EnemyConfig enemyConfig;
    }
}