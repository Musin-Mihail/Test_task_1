using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Enemy;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public class PlayerFinisher : MonoBehaviour
    {
        public EnemyFinisherHandler enemyFinisherHandler;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private PlayerAnimation _playerAnimation;
        private PlayerMovement _playerMovement;
        private PlayerEquipment _playerEquipment;
        private Collider _playerCollider;
        private bool _isFinishing;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerCollider">Ссылка на Collider игрока.</param>
        /// <param name="playerAnimation">Ссылка на PlayerAnimation.</param>
        /// <param name="playerMovement">Ссылка на PlayerMovement.</param>
        /// <param name="playerEquipment">Ссылка на PlayerEquipment.</param>
        public void Initialize(Collider playerCollider, PlayerAnimation playerAnimation, PlayerMovement playerMovement, PlayerEquipment playerEquipment)
        {
            _playerCollider = playerCollider;
            _playerAnimation = playerAnimation;
            _playerMovement = playerMovement;
            _playerEquipment = playerEquipment;
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Sword, false);
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
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Gun, false);
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Sword, true);
            _playerAnimation.PlayAnimation(PlayerAnimationNames.Finishing);
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(enemyFinisherHandler.RepositionEnemyCoroutine());
            yield return new WaitForSeconds(1.2f);
            _playerCollider.enabled = true;
        }

        private IEnumerator ResetFinisherStateAndTriggerEnemyHandler()
        {
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(PlayerEquipment.WeaponType.Sword, false);
            _isFinishing = false;
            _playerMovement.RotateTowardsCamera();
            yield return null;
        }
    }
}