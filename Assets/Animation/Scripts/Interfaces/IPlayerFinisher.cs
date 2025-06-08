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
        event Action OnFinisherStateReset;

        Vector3 TargetPosition { get; set; }
        bool IsFinishing();
        void SetFinishing(bool isFinishing);

        void AnimationEvent_FinisherImpactPoint();
        void AnimationEvent_FinisherComplete();
        void ResetFinisherState();
    }
}