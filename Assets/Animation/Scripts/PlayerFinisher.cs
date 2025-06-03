using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    public class PlayerFinisher : MonoBehaviour
    {
        public GameObject gun;
        public GameObject sword;
        public Animator enemyAnimator;
        public GameObject enemy;
        private PlayerAnimation _playerAnimation;
        private PlayerMovement _playerMovement;
        private Collider _playerCollider;

        private bool _isFinishing;
        public Vector3 TargetPosition { get; set; }

        public bool IsFinishing() => _isFinishing;

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
            var distanceToTarget = Vector3.Distance(transform.position, TargetPosition);
            while (distanceToTarget > 2.5f)
            {
                transform.position += transform.forward * (5 * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transform.position, TargetPosition);
                yield return null;
            }

            gun.SetActive(false);
            sword.SetActive(true);
            _playerAnimation.PlayAnimation(PlayerAnimationNames.Finishing);
            yield return new WaitForSeconds(0.35f);

            enemyAnimator.enabled = false;
            yield return new WaitForSeconds(1.15f);

            gun.SetActive(true);
            sword.SetActive(false);
            _isFinishing = false;
            _playerMovement.RotateTowardsCamera();

            yield return new WaitForSeconds(4.0f);
            enemy.SetActive(false);
            yield return new WaitForSeconds(1.0f);

            while (true)
            {
                var randomVector = Random.insideUnitCircle * 10;
                var newVector = new Vector3(randomVector.x + transform.position.x, 0, randomVector.y + transform.position.z);
                var distance = Vector3.Distance(transform.position, newVector);
                if (distance > 6)
                {
                    enemy.transform.position = newVector;
                    break;
                }

                yield return new WaitForSeconds(0.1f);
            }

            enemyAnimator.enabled = true;
            enemy.SetActive(true);
            _playerCollider.enabled = true;
        }
    }
}