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
        event Action OnFinisherAnimationFullyCompleted;

        Vector3 TargetPosition { get; set; }
        bool IsFinishing();

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerCollider">Ссылка на Collider игрока.</param>
        /// <param name="playerAnimation">Ссылка на IPlayerAnimation.</param>
        /// <param name="playerEquipment">Ссылка на IPlayerEquipment.</param>
        /// <param name="playerRotator">Ссылка на IPlayerEquipment.</param>
        void Initialize(Collider playerCollider, IPlayerAnimation playerAnimation, IPlayerEquipment playerEquipment, IPlayerRotator playerRotator);

        /// <summary>
        /// Запуск процесса добивания.
        /// </summary>
        void StartFinishingSequence();

        void AnimationEvent_FinisherImpactPoint();
        void AnimationEvent_FinisherComplete();
    }
}