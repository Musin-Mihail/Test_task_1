using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerMovement
    {
        Vector3 CurrentMovementInput { get; }
        void Initialize(IPlayerController controller, IPlayerFinisher finisher, IPlayerAnimationController animationController, PlayerMovementState movementState);

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
        /// Возвращает, движется ли игрок в данный момент.
        /// </summary>
        bool IsMoving();

        /// <summary>
        /// Обновляет анимацию движения игрока через PlayerAnimationController.
        /// </summary>
        void UpdateMovementAndAnimation();
    }
}