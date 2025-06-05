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

        /// <summary>
        /// Инициализирует обработчик добивания противника и подписывается на событие PlayerFinisher.
        /// </summary>
        /// <param name="playerFinisher">Ссылка на IPlayerFinisher.</param>
        public void Initialize(IPlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
            if (_playerFinisher != null)
            {
                _playerFinisher.OnFinisherSequenceCompleted += RepositionEnemyOnFinisherCompleted;
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
                _playerFinisher.OnFinisherSequenceCompleted -= RepositionEnemyOnFinisherCompleted;
            }
        }

        /// <summary>
        /// Метод, вызываемый при завершении последовательности добивания игроком.
        /// </summary>
        private void RepositionEnemyOnFinisherCompleted()
        {
            StartCoroutine(RepositionEnemyCoroutine());
        }

        private IEnumerator RepositionEnemyCoroutine()
        {
            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = false;
            }

            yield return new WaitForSeconds(1.15f);

            if (enemy)
            {
                enemy.SetActive(false);
            }

            yield return new WaitForSeconds(4.0f);

            while (true)
            {
                var randomVector = Random.insideUnitCircle * 10;
                var newVector = new Vector3(randomVector.x + transform.position.x, 0, randomVector.y + transform.position.z);
                var distance = Vector3.Distance(transform.position, newVector);
                if (distance > 6)
                {
                    transform.position = newVector;
                    break;
                }

                yield return new WaitForSeconds(0.1f);
            }

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