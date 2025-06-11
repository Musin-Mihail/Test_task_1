using UnityEngine;

namespace Animation.Scripts.Configs
{
    /// <summary>
    /// Конфигурация для движения игрока.
    /// </summary>
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/Movement Config")]
    public class MovementConfig : ScriptableObject
    {
        [Header("Настройки движения")]
        [Tooltip("Скорость перемещения игрока.")]
        public float speed = 10f;

        [Tooltip("Время сглаживания анимации движения.")]
        public float animationSmoothTime = 0.1f;
    }
}