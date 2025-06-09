using System.Collections;
using Animation.Scripts.Common;
using Animation.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    public class EnemyLifecycleManager
    {
        private readonly GameObject _enemyObject;
        private readonly Transform _enemyTransform;
        private readonly Animator _enemyAnimator;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Transform _playerTransform;

        public EnemyLifecycleManager(GameObject enemyObject, Transform enemyTransform, Animator enemyAnimator, ICoroutineRunner coroutineRunner, [Inject] PlayerFacade playerFacade)
        {
            _enemyObject = enemyObject;
            _enemyTransform = enemyTransform;
            _enemyAnimator = enemyAnimator;
            _coroutineRunner = coroutineRunner;
            _playerTransform = playerFacade.transform;
        }

        public void Respawn()
        {
            _coroutineRunner.StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            _enemyObject.SetActive(false);
            yield return new WaitForSeconds(3.0f);

            var randomDirection = Random.insideUnitCircle.normalized;
            var newPosition = _playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * 6f;

            _enemyTransform.position = newPosition;
            _enemyObject.SetActive(true);
            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = true;
            }
        }
    }
}