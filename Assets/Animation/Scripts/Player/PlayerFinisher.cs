using System;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// Теперь реализует IAnimationEventHandler для обработки событий анимации.
    /// </summary>
    public class PlayerFinisher : IPlayerFinisher
    {
        public event Action OnFinisherSequenceCompleted;
        public event Action OnFinisherAnimationFullyCompleted;
        public event Action OnFinisherStateReset;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private readonly Collider _playerCollider;
        private bool _isFinishing;

        private const string FinisherImpactPointEvent = "FinisherImpactPoint";
        private const string FinisherCompleteEvent = "FinisherComplete";

        [Inject]
        public PlayerFinisher(Collider playerCollider)
        {
            _playerCollider = playerCollider;
        }

        public void SetFinishing(bool isFinishing)
        {
            _isFinishing = isFinishing;
            _playerCollider.enabled = !isFinishing; // Отключаем коллайдер игрока во время добивания
        }

        /// <summary>
        /// Универсальный метод для обработки событий анимации.
        /// Вызывается AnimationEventBridge.
        /// </summary>
        /// <param name="eventName">Имя события из Animation Event.</param>
        public void HandleAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case FinisherImpactPointEvent:
                    OnFinisherSequenceCompleted?.Invoke();
                    break;
                case FinisherCompleteEvent:
                    OnFinisherAnimationFullyCompleted?.Invoke();
                    break;
                default:
                    Debug.LogWarning($"Unhandled animation event: {eventName}");
                    break;
            }
        }

        public void ResetFinisherState()
        {
            _isFinishing = false;
            OnFinisherStateReset?.Invoke();
            Debug.Log("Finisher state reset.");
        }
    }
}