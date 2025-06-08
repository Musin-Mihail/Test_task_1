using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerMovement
    {
        Vector3 CurrentMovementInput { get; }

        /// <summary>
        /// Выполняет движение игрока. Вызывается из состояния FSM.
        /// </summary>
        void Move();

        /// <summary>
        /// Возвращает, движется ли игрок в данный момент.
        /// </summary>
        bool IsMoving();
    }
}