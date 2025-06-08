using System;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    public class EnemyFinisherHandler : IDisposable
    {
        private readonly Animator _enemyAnimator;
        private readonly SignalBus _signalBus;
        private readonly EnemyLifecycleManager _enemyLifecycleManager;

        [Inject]
        public EnemyFinisherHandler(
            [Inject(Id = "EnemyGameObject")] GameObject enemy,
            SignalBus signalBus,
            EnemyLifecycleManager enemyLifecycleManager
        )
        {
            _signalBus = signalBus;
            _enemyLifecycleManager = enemyLifecycleManager;
            _enemyAnimator = enemy.GetComponent<Animator>();

            _signalBus.Subscribe<FinisherImpactSignal>(HandleFinisherImpact);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(HandleFinisherAnimationComplete);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<FinisherImpactSignal>(HandleFinisherImpact);
            _signalBus.Unsubscribe<FinisherAnimationCompleteSignal>(HandleFinisherAnimationComplete);
        }

        /// <summary>
        /// Метод, вызываемый при достижении точки удара в последовательности добивания игроком.
        /// </summary>
        private void HandleFinisherImpact()
        {
            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = false;
            }
        }

        /// <summary>
        /// Метод, вызываемый при полном завершении анимации добивания игроком.
        /// </summary>
        private void HandleFinisherAnimationComplete()
        {
            _enemyLifecycleManager.RespawnEnemy();
        }
    }
}