using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerMovement
    {
        bool IsMovingForward { get; }
        bool IsMovingBack { get; }
        bool IsMovingLeft { get; }
        bool IsMovingRight { get; }

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="controller">Ссылка на PlayerController.</param>
        /// <param name="finisher">Ссылка на PlayerFinisher.</param>
        /// <param name="animationController">Ссылка на PlayerAnimationController.</param>
        void Initialize(IPlayerController controller, IPlayerFinisher finisher, IPlayerAnimationController animationController);

        /// <summary>
        /// Выполняет движение игрока. Вызывается из состояния FSM.
        /// </summary>
        void Move();

        /// <summary>
        /// Поворачивает объект игрока в сторону указателя мыши.
        /// </summary>
        void RotationToMouse();

        /// <summary>
        /// Поворачивает объект игрока в сторону цели.
        /// </summary>
        void RotationToTarget();

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
        void RotateTowardsCamera();

        /// <summary>
        /// Перемещает игрока к указанной цели до достижения заданного расстояния.
        /// </summary>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <param name="stopDistance">Расстояние до цели, на котором нужно остановиться.</param>
        IEnumerator MoveToTarget(Vector3 targetPosition, float stopDistance);

        /// <summary>
        /// Возвращает, движется ли игрок в данный момент.
        /// </summary>
        bool IsMoving();

        /// <summary>
        /// Обновляет анимацию движения игрока через PlayerAnimationController.
        /// </summary>
        void UpdateMovementAndAnimation();
    }
}