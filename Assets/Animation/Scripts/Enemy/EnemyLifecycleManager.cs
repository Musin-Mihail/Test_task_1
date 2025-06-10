using System.Collections;
using Animation.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    public class EnemyLifecycleManager : MonoBehaviour
    {
        private Transform _playerTransform;
        private EnemyConfig _config;
        private Animator _enemyAnimator;
        private Collider _enemyCollider;

        [Inject]
        public void Construct([Inject(Id = "PlayerTransform")] Transform playerTransform, EnemyConfig config, Animator enemyAnimator)
        {
            _playerTransform = playerTransform;
            _config = config;
            _enemyAnimator = enemyAnimator;
            _enemyCollider = GetComponent<Collider>();
        }

        public void Respawn()
        {
            StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            _enemyCollider.enabled = false;
            yield return new WaitForSeconds(_config.respawnDelay);

            var randomDirection = Random.insideUnitCircle.normalized;
            var newPosition = _playerTransform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * _config.respawnDistance;

            transform.position = newPosition;
            if (_enemyAnimator)
            {
                _enemyAnimator.enabled = true;
            }

            _enemyCollider.enabled = true;
        }
    }
}