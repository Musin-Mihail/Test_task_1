using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Enemy;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public class PlayerFinisher : MonoBehaviour
    {
        public GameObject gun;
        public GameObject sword;
        public EnemyFinisherHandler enemyFinisherHandler;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private PlayerAnimation _playerAnimation;
        private PlayerMovement _playerMovement;
        private Collider _playerCollider;
        private bool _isFinishing;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
            _playerAnimation = GetComponent<PlayerAnimation>();
            _playerMovement = GetComponent<PlayerMovement>();
            gun.SetActive(true);
            sword.SetActive(false);
        }

        public void StartFinishingSequence()
        {
            _isFinishing = true;
            _playerCollider.enabled = false;
            _playerAnimation.PlayAnimation(PlayerAnimationNames.RunRifle);
            StartCoroutine(FinishingCoroutine());
        }

        private IEnumerator FinishingCoroutine()
        {
            yield return StartCoroutine(MoveToTarget());
            yield return StartCoroutine(PerformFinishingAnimation());
            yield return StartCoroutine(ResetFinisherStateAndTriggerEnemyHandler());
            yield return StartCoroutine(enemyFinisherHandler.RepositionEnemyCoroutine());
            _playerCollider.enabled = true;
        }

        private IEnumerator MoveToTarget()
        {
            var distanceToTarget = Vector3.Distance(transform.position, TargetPosition);
            while (distanceToTarget > 2.5f)
            {
                transform.position += transform.forward * (5 * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transform.position, TargetPosition);
                yield return null;
            }
        }

        private IEnumerator PerformFinishingAnimation()
        {
            gun.SetActive(false);
            sword.SetActive(true);
            _playerAnimation.PlayAnimation(PlayerAnimationNames.Finishing);
            yield return new WaitForSeconds(1.6f);
        }

        private IEnumerator ResetFinisherStateAndTriggerEnemyHandler()
        {
            gun.SetActive(true);
            sword.SetActive(false);
            _isFinishing = false;
            _playerMovement.RotateTowardsCamera();
            yield return null;
        }
    }
}