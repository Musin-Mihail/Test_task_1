using System;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления логикой добивания игрока.
    /// Теперь наследует IAnimationEventHandler для универсальной обработки событий анимации.
    /// </summary>
    public interface IPlayerFinisher : IAnimationEventHandler
    {
        event Action OnFinisherStateReset;

        Vector3 TargetPosition { get; set; }
        bool IsFinishing();
        void SetFinishing(bool isFinishing);
        void ResetFinisherState();
    }
}