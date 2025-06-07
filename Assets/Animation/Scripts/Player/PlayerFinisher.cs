using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// </summary>
    public class PlayerFinisher : MonoBehaviour, IPlayerFinisher
    {
        [SerializeField, Tooltip("Дистанция до цели для начала добивания")]
        private float finishingStartDistance = 2.5f;

        [SerializeField, Tooltip("Время анимации до удара")]
        private float timeBeforeImpact = 0.4f;

        [SerializeField, Tooltip("Время на выполнение анимации удара")]
        private float finishingStrikeDuration = 1.2f;

        [SerializeField, Tooltip("Скорость перемещения во время добивания")]
        private float finishingMovementSpeed = 5f;

        public event Action OnFinisherSequenceCompleted;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private IPlayerAnimation _playerAnimation;
        private IPlayerMovement _playerMovement;
        private IPlayerEquipment _playerEquipment;
        private Collider _playerCollider;
        private bool _isFinishing;

        public void Initialize(Collider playerCollider, IPlayerAnimation playerAnimation, IPlayerMovement playerMovement, IPlayerEquipment playerEquipment)
        {
            _playerCollider = playerCollider;
            _playerAnimation = playerAnimation;
            _playerMovement = playerMovement;
            _playerEquipment = playerEquipment;
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);
        }

        public void StartFinishingSequence()
        {
            _isFinishing = true;
            _playerCollider.enabled = false;
            _playerAnimation.SetBool("IsMoving", true);

            StartCoroutine(FinishingCoroutine());
        }

        private IEnumerator FinishingCoroutine()
        {
            yield return StartCoroutine(MoveToTarget(transform, TargetPosition, finishingStartDistance, finishingMovementSpeed));
            yield return StartCoroutine(PerformFinishingAnimation());
            yield return StartCoroutine(ResetFinisherState());
        }

        private IEnumerator PerformFinishingAnimation()
        {
            _playerAnimation.SetBool("IsMoving", false);
            _playerEquipment.SetWeaponActive(WeaponType.Gun, false);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, true);
            _playerAnimation.SetBool("Finisher", true);
            yield return new WaitForSeconds(timeBeforeImpact);
            OnFinisherSequenceCompleted?.Invoke();
            yield return new WaitForSeconds(finishingStrikeDuration);
            _playerCollider.enabled = true;
            _playerAnimation.SetBool("Finisher", false);
        }

        private IEnumerator ResetFinisherState()
        {
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);
            _isFinishing = false;
            _playerMovement.RotateTowardsCamera();
            yield return null;
        }

        /// <summary>
        /// Реализация MoveToTarget из IMovementService.
        /// </summary>
        private IEnumerator MoveToTarget(Transform targetTransform, Vector3 targetPosition, float stopDistance, float speed)
        {
            Debug.Log(targetTransform.position);
            Debug.Log(targetPosition);
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