using System;
using Animation.Scripts.Interfaces;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class PlayerFinisher : IPlayerFinisher
    {
        public event Action OnFinisherStateReset;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private readonly Collider _playerCollider;
        private readonly SignalBus _signalBus;
        private bool _isFinishing;

        private const string FinisherImpactPointEvent = "FinisherImpactPoint";
        private const string FinisherCompleteEvent = "FinisherComplete";

        [Inject]
        public PlayerFinisher(Collider playerCollider, SignalBus signalBus)
        {
            _playerCollider = playerCollider;
            _signalBus = signalBus;
        }

        public void SetFinishing(bool isFinishing)
        {
            _isFinishing = isFinishing;
            _playerCollider.enabled = !isFinishing;
        }

        public void HandleAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case FinisherImpactPointEvent:
                    _signalBus.Fire<FinisherImpactSignal>();
                    break;
                case FinisherCompleteEvent:
                    _signalBus.Fire<FinisherAnimationCompleteSignal>();
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
        }
    }
}