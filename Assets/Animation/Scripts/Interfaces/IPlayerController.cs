using System;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для обработки пользовательского ввода и передачи команд игрока.
    /// </summary>
    public interface IPlayerController
    {
        public event Action<Vector2> OnMovementInput;
        public event Action OnSpacePressed;

        /// <summary>
        /// Возвращает текущую позицию мыши в мировых координатах.
        /// </summary>
        Vector3 GetMouseWorldPosition();
    }
}