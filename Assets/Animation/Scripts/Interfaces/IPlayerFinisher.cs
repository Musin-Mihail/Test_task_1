using System;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления логикой добивания игрока.
    /// </summary>
    public interface IPlayerFinisher
    {
        event Action OnFinisherSequenceCompleted;

        Vector3 TargetPosition { get; set; }
        bool IsFinishing();
        void Initialize(Collider playerCollider, IPlayerAnimation playerAnimation, IPlayerMovement playerMovement, IPlayerEquipment playerEquipment);
        void StartFinishingSequence();
    }
}