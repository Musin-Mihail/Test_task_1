using System.Collections;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    /// <summary>
    /// Управляет жизненным циклом врага, включая его респаун.
    /// </summary>
    public class EnemyLifecycleManager
    {
        private readonly GameObject _enemy;
        private readonly Transform _playerTransform;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Animator _enemyAnimator;

        [Inject]
        public EnemyLifecycleManager(
            [Inject(Id = "EnemyGameObject")] GameObject enemy,
            [Inject(Id = "PlayerTransform")] Transform playerTransform,
            ICoroutineRunner coroutineRunner
        )
        {
            _enemy = enemy;
            _playerTransform = playerTransform;
            _coroutineRunner = coroutineRunner;
            _enemyAnimator = _enemy.GetComponent<Animator>();
        }

        /// <summary>
        /// Запускает процесс респауна врага.
        /// </summary>
        public void RespawnEnemy()
        {
            _coroutineRunner.StartCoroutine(RepositionEnemyCoroutine());
        }

        private IEnumerator RepositionEnemyCoroutine()
        {
            if (_enemy)
            {
                _enemy.SetActive(false);
            }

            yield return new WaitForSeconds(4.0f);

            var randomDirection = Random.insideUnitCircle.normalized;
            var newPosition = _playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * 6f;

            if (_enemy)
            {
                _enemy.transform.position = newPosition;
                _enemy.SetActive(true);
            }

            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = true;
            }
        }
    }
}