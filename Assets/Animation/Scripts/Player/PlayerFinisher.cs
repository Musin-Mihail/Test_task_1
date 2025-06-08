using System;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// Теперь является более "тонким" классом, отслеживающим события анимации.
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

        [Inject]
        public PlayerFinisher(Collider playerCollider)
        {
            _playerCollider = playerCollider;
        }

        public void SetFinishing(bool isFinishing)
        {
            _isFinishing = isFinishing;
            _playerCollider.enabled = !isFinishing;
        }

        /// <summary>
        /// Метод, вызываемый Animation Event в точке удара добивания.
        /// </summary>
        public void AnimationEvent_FinisherImpactPoint()
        {
            OnFinisherSequenceCompleted?.Invoke();
        }

        /// <summary>
        /// Метод, вызываемый Animation Event по завершении анимации добивания.
        /// </summary>
        public void AnimationEvent_FinisherComplete()
        {
            OnFinisherAnimationFullyCompleted?.Invoke();
        }

        public void ResetFinisherState()
        {
            _isFinishing = false;
            OnFinisherStateReset?.Invoke();
        }
    }
}