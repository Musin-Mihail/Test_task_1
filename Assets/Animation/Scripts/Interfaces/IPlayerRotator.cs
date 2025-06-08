using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления вращением игрока.
    /// </summary>
    public interface IPlayerRotator
    {
        /// <summary>
        /// Поворачивает объект игрока в сторону указателя мыши.
        /// </summary>
        void RotationToMouse();

        /// <summary>
        /// Поворачивает объект игрока в сторону цели.
        /// </summary>
        ///  <param name="targetPosition">Цель для разворота.</param>
        void RotationToTarget(Vector3 targetPosition);

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
        void RotateTowardsCamera();
    }
}