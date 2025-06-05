using System;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для обработки пользовательского ввода и передачи команд игрока.
    /// </summary>
    public interface IPlayerController
    {
        event Action OnMoveForwardPressed;
        event Action OnMoveForwardReleased;
        event Action<string> OnMoveForwardKeyDown;
        event Action OnMoveBackPressed;
        event Action OnMoveBackReleased;
        event Action<string> OnMoveBackKeyDown;
        event Action OnMoveLeftPressed;
        event Action OnMoveLeftReleased;
        event Action<string> OnMoveLeftKeyDown;
        event Action OnMoveRightPressed;
        event Action OnMoveRightReleased;
        event Action<string> OnMoveRightKeyDown;
        event Action OnSpacePressed;

        Vector3 GetMouseWorldPosition();
    }
}