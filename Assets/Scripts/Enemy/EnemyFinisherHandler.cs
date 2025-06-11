using System;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    public class EnemyFinisherHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly Animator _animator;
        private readonly EnemyLifecycleManager _lifecycleManager;

        public EnemyFinisherHandler(SignalBus signalBus, Animator animator, EnemyLifecycleManager lifecycleManager)
        {
            _signalBus = signalBus;
            _animator = animator;
            _lifecycleManager = lifecycleManager;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<FinisherImpactPointReachedSignal>(OnFinisherImpact);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<FinisherImpactPointReachedSignal>(OnFinisherImpact);
            _signalBus.Unsubscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
        }

        private void OnFinisherImpact()
        {
            if (_animator) _animator.enabled = false;
        }

        private void OnFinisherAnimationComplete()
        {
            _lifecycleManager.Respawn();
        }
    }
}