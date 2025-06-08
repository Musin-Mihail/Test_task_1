using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса, отвечающего за перемещение Transform к цели.
    /// </summary>
    public interface ITargetMover
    {
        /// <summary>
        /// Перемещает указанный Transform к целевой позиции.
        /// </summary>
        /// <param name="transformToMove">Transform, который нужно переместить.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <param name="stopDistance">Дистанция, на которой нужно остановиться от цели.</param>
        /// <param name="speed">Скорость перемещения.</param>
        /// <returns>Корутина, которая завершится после достижения цели.</returns>
        IEnumerator MoveToTarget(Transform transformToMove, Vector3 targetPosition, float stopDistance, float speed);
    }
}