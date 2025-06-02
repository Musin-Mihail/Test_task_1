using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за логику боя игрока, включая добивания и смену оружия.
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        public GameObject gun;
        public GameObject sword;
        public GameObject enemy;
        public Animator enemyAnimator;
        public GameObject finishingText;

        private Collider _playerCollider;
        private PlayerStateMachine _playerStateMachine;
        private bool _isFinishing;

        public Vector3 CurrentTarget { get; private set; }

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            gun.SetActive(true);
            sword.SetActive(false);
        }

        /// <summary>
        /// Обработчик нажатия пробела.
        /// </summary>
        public void StartFinishing()
        {
            _isFinishing = true;
            _playerCollider.enabled = false;
            finishingText.SetActive(false);
            _playerStateMachine.PlayerAnimation.PlayAnimation("Run_Rifle");
            StartCoroutine(FinishingCoroutine());
        }

        /// <summary>
        /// Корутина, управляющая последовательностью событий добивания.
        /// </summary>
        private IEnumerator FinishingCoroutine()
        {
            var distanceToTarget = Vector3.Distance(transform.position, CurrentTarget);
            while (distanceToTarget > 2.5f)
            {
                transform.position += transform.forward * (5 * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transform.position, CurrentTarget);
                yield return null;
            }

            gun.SetActive(false);
            sword.SetActive(true);
            _playerStateMachine.PlayerAnimation.PlayAnimation("Finishing");
            yield return new WaitForSeconds(0.35f);

            enemyAnimator.enabled = false;
            yield return new WaitForSeconds(1.15f);

            gun.SetActive(true);
            sword.SetActive(false);
            _isFinishing = false;
            _playerStateMachine.PlayerMovement.RotateTowardsCamera();

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

        /// <summary>
        /// Вызывается при входе в триггер.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                CurrentTarget = other.gameObject.transform.position;
                finishingText.SetActive(true);
            }
        }

        /// <summary>
        /// Вызывается при выходе из триггера.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                finishingText.SetActive(false);
            }
        }

        /// <summary>
        /// Возвращает, находится ли игрок в состоянии добивания.
        /// </summary>
        public bool IsFinishing()
        {
            return _isFinishing;
        }
    }
}