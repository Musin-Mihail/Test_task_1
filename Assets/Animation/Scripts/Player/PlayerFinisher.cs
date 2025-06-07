using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// </summary>
    public class PlayerFinisher : MonoBehaviour, IPlayerFinisher
    {
        [SerializeField] private PlayerConfig playerConfig;

        public event Action OnFinisherSequenceCompleted;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private IPlayerAnimation _playerAnimation;
        private IPlayerMovement _playerMovement;
        private IPlayerEquipment _playerEquipment;
        private IPlayerRotator _playerRotator;
        private Collider _playerCollider;
        private bool _isFinishing;

        public void Initialize(Collider playerCollider, IPlayerAnimation playerAnimation, IPlayerMovement playerMovement, IPlayerEquipment playerEquipment, IPlayerRotator playerRotator)
        {
            _playerCollider = playerCollider;
            _playerAnimation = playerAnimation;
            _playerMovement = playerMovement;
            _playerEquipment = playerEquipment;
            _playerRotator = playerRotator;
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);

            if (!playerConfig)
            {
                Debug.LogError("PlayerConfig не назначен в инспекторе PlayerFinisher. Пожалуйста, назначьте его.");
                enabled = false;
            }
        }

        public void StartFinishingSequence()
        {
            if (!playerConfig) return;

            _isFinishing = true;
            _playerCollider.enabled = false;
            _playerAnimation.SetBool("IsMoving", true);

            StartCoroutine(FinishingCoroutine());
        }

        private IEnumerator FinishingCoroutine()
        {
            if (!playerConfig) yield break;

            yield return StartCoroutine(MoveToTarget(transform, TargetPosition, playerConfig.finishingStartDistance, playerConfig.finishingMovementSpeed));
            yield return StartCoroutine(PerformFinishingAnimation());
            yield return StartCoroutine(ResetFinisherState());
        }

        private IEnumerator PerformFinishingAnimation()
        {
            if (!playerConfig) yield break;

            _playerAnimation.SetBool("IsMoving", false);
            _playerEquipment.SetWeaponActive(WeaponType.Gun, false);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, true);
            _playerAnimation.SetBool("Finisher", true);
            yield return new WaitForSeconds(playerConfig.timeBeforeImpact);
            OnFinisherSequenceCompleted?.Invoke();
            yield return new WaitForSeconds(playerConfig.finishingStrikeDuration);
            _playerCollider.enabled = true;
            _playerAnimation.SetBool("Finisher", false);
        }

        private IEnumerator ResetFinisherState()
        {
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);
            _isFinishing = false;
            _playerRotator.RotateTowardsCamera();
            yield return null;
        }

        /// <summary>
        /// Реализация MoveToTarget из IMovementService.
        /// </summary>
        private IEnumerator MoveToTarget(Transform targetTransform, Vector3 targetPosition, float stopDistance, float speed)
        {
            var distanceToTarget = Vector3.Distance(targetTransform.position, targetPosition);
            while (distanceToTarget > stopDistance)
            {
                var directionToTarget = (targetPosition - targetTransform.position).normalized;
                targetTransform.position += directionToTarget * (speed * Time.deltaTime);
                distanceToTarget = Vector3.Distance(targetTransform.position, targetPosition);
                yield return null;
            }
        }
    }
}