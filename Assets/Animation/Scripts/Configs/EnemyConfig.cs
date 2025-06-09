using UnityEngine;

namespace Animation.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Настройки респауна")]
        [Tooltip("Время в секундах до респауна.")]
        public float respawnDelay = 3.0f;

        [Tooltip("Дистанция от игрока для респауна.")]
        public float respawnDistance = 6.0f;
    }
}