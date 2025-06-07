using System.Collections;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation.Scripts.Enemy
{
    /// <summary>
    /// Отвечает за логику обработки добивания противника.
    /// Реализует интерфейс IEnemyFinisherHandler.
    /// </summary>
    public class EnemyFinisherHandler : MonoBehaviour, IEnemyFinisherHandler
    {
        public GameObject enemy;

        private Animator _enemyAnimator;
        private IPlayerFinisher _playerFinisher;

        private void Awake()
        {
            _enemyAnimator = enemy.GetComponent<Animator>();
        }

        public void Initialize(IPlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
            if (_playerFinisher != null)
            {
                _playerFinisher.OnFinisherSequenceCompleted += HandleFinisherImpact;
                _playerFinisher.OnFinisherAnimationFullyCompleted += HandleFinisherAnimationComplete;
            }
            else
            {
                Debug.LogError("PlayerFinisher is null in EnemyFinisherHandler.Initialize.");
            }
        }

        private void OnDestroy()
        {
            if (_playerFinisher != null)
            {
                _playerFinisher.OnFinisherSequenceCompleted -= HandleFinisherImpact;
                _playerFinisher.OnFinisherAnimationFullyCompleted -= HandleFinisherAnimationComplete;
            }
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
            StartCoroutine(RepositionEnemyCoroutine());
        }

        /// <summary>
        /// Корутина для перемещения врага в новую случайную позицию после завершения добивания.
        /// </summary>
        private IEnumerator RepositionEnemyCoroutine()
        {
            if (enemy)
            {
                enemy.SetActive(false);
            }

            yield return new WaitForSeconds(4.0f);

            var randomDirection = Random.insideUnitCircle.normalized;
            var newPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * 6f;

            transform.position = newPosition;

            if (enemy)
            {
                enemy.SetActive(true);
            }

            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = true;
            }
        }
    }
}