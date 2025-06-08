using System;
using System.Collections;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Animation.Scripts.Enemy
{
    /// <summary>
    /// Отвечает за логику обработки добивания противника.
    /// </summary>
    public class EnemyFinisherHandler : IDisposable
    {
        private readonly Animator _enemyAnimator;
        private readonly IPlayerFinisher _playerFinisher;
        private readonly GameObject _enemy;
        private readonly Transform _playerTransform;
        private readonly ICoroutineRunner _coroutineRunner;

        [Inject]
        public EnemyFinisherHandler(
            IPlayerFinisher playerFinisher,
            [Inject(Id = "EnemyGameObject")] GameObject enemy,
            [Inject(Id = "PlayerTransform")] Transform playerTransform,
            ICoroutineRunner coroutineRunner
        )
        {
            _enemy = enemy;
            _playerFinisher = playerFinisher;
            _playerTransform = playerTransform;
            _coroutineRunner = coroutineRunner;

            _playerFinisher.OnFinisherSequenceCompleted += HandleFinisherImpact;
            _playerFinisher.OnFinisherAnimationFullyCompleted += HandleFinisherAnimationComplete;
            _enemyAnimator = _enemy.GetComponent<Animator>();
        }

        public void Dispose()
        {
            _playerFinisher.OnFinisherSequenceCompleted -= HandleFinisherImpact;
            _playerFinisher.OnFinisherAnimationFullyCompleted -= HandleFinisherAnimationComplete;
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

            Debug.Log("EnemyFinisherHandler: Player finisher impact received!");
        }

        /// <summary>
        /// Метод, вызываемый при полном завершении анимации добивания игроком.
        /// Здесь враг будет деактивирован.
        /// </summary>
        private void HandleFinisherAnimationComplete()
        {
            _coroutineRunner.StartCoroutine(RepositionEnemyCoroutine());
        }

        /// <summary>
        /// Корутина для перемещения врага в новую случайную позицию после завершения добивания.
        /// </summary>
        private IEnumerator RepositionEnemyCoroutine()
        {
            if (_enemy)
            {
                _enemy.SetActive(false);
            }

            yield return new WaitForSeconds(4.0f);

            var randomDirection = Random.insideUnitCircle.normalized;
            var newPosition = _playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * 6f;

            _playerTransform.position = newPosition;

            if (_enemy)
            {
                _enemy.SetActive(true);
            }

            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = true;
            }
        }
    }
}