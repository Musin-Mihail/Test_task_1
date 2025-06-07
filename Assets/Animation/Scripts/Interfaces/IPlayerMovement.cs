using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerMovement
    {
        Vector3 CurrentMovementInput { get; }

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="controller">Ссылка на IPlayerController.</param>
        /// <param name="animationController">Ссылка на IPlayerAnimationController.</param>
        /// <param name="movementState">Ссылка на PlayerMovementState.</param>
        void Initialize(IPlayerController controller, IPlayerAnimationController animationController, PlayerMovementState movementState);

        /// <summary>
        /// Выполняет движение игрока. Вызывается из состояния FSM.
        /// </summary>
        void Move();

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