using UnityEngine;

namespace Animation.Scripts.ScriptableObjects
{
    /// <summary>
    /// Конфигурация для системы добиваний.
    /// </summary>
    [CreateAssetMenu(fileName = "FinisherConfig", menuName = "Configs/Finisher Config", order = 2)]
    public class FinisherConfig : ScriptableObject
    {
        [Header("Настройки добивания")]
        [Tooltip("Дистанция до цели для начала добивания.")]
        public float finishingStartDistance = 2.5f;

        [Tooltip("Скорость перемещения во время добивания.")]
        public float finishingMovementSpeed = 5f;
    }
}