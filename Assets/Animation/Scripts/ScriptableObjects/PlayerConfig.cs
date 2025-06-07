using UnityEngine;

namespace Animation.Scripts.ScriptableObjects
{
    /// <summary>
    /// ScriptableObject для хранения всех конфигурационных данных игрока.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/Player Config", order = 1)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Настройки движения")]
        [Tooltip("Скорость перемещения игрока.")]
        public float speed = 10f;

        [Tooltip("Время сглаживания анимации движения.")]
        public float animationSmoothTime = 0.1f;

        [Header("Настройки добивания")]
        [Tooltip("Дистанция до цели для начала добивания.")]
        public float finishingStartDistance = 2.5f;

        [Tooltip("Время анимации до удара в добивании.")]
        public float timeBeforeImpact = 0.4f;

        [Tooltip("Время на выполнение анимации удара в добивании.")]
        public float finishingStrikeDuration = 1.2f;

        [Tooltip("Скорость перемещения во время добивания.")]
        public float finishingMovementSpeed = 5f;

        [Header("Настройки камеры")]
        [Tooltip("Смещение камеры относительно игрока.")]
        public Vector3 cameraOffset = new(6.0f, 10.0f, -6.0f);
    }
}