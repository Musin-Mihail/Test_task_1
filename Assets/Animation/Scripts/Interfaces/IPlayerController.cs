using System;
using Animation.Scripts.Constants;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для обработки пользовательского ввода и передачи команд игрока.
    /// </summary>
    public interface IPlayerController
    {
        public event Action<MovementDirection, KeyState> OnMovementIntent;
        public event Action OnSpacePressed;

        Vector3 GetMouseWorldPosition();
    }
}