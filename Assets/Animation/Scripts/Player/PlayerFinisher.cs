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
        [SerializeField] private Transform player;

        public event Action OnFinisherSequenceCompleted;
        public event Action OnFinisherAnimationFullyCompleted;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private IPlayerAnimation _playerAnimation;
        private IPlayerEquipment _playerEquipment;
        private IPlayerRotator _playerRotator;
        private Collider _playerCollider;
        private bool _isFinishing;

        private bool _isImpactPointReached;
        private bool _isAnimationCompleted;

        public void Initialize(Collider playerCollider, IPlayerAnimation playerAnimation, IPlayerEquipment playerEquipment, IPlayerRotator playerRotator)
        {
            _playerCollider = playerCollider;
            _playerAnimation = playerAnimation;
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

            _isImpactPointReached = false;
            _isAnimationCompleted = false;

            StartCoroutine(FinishingCoroutine());
        }

        /// <summary>
        /// Метод, вызываемый Animation Event в точке удара добивания.
        /// </summary>
        public void AnimationEvent_FinisherImpactPoint()
        {
            _isImpactPointReached = true;
            OnFinisherSequenceCompleted?.Invoke();
            Debug.Log("Animation Event: Finisher Impact Point reached!");
        }

        /// <summary>
        /// Метод, вызываемый Animation Event по завершении анимации добивания.
        /// </summary>
        public void AnimationEvent_FinisherComplete()
        {
            _isAnimationCompleted = true;
            OnFinisherAnimationFullyCompleted?.Invoke();
            Debug.Log("Animation Event: Finisher animation completed!");
        }


        private IEnumerator FinishingCoroutine()
        {
            if (!playerConfig) yield break;

            yield return StartCoroutine(MoveToTarget(player, TargetPosition, playerConfig.finishingStartDistance, playerConfig.finishingMovementSpeed));
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

            yield return new WaitUntil(() => _isImpactPointReached);
            yield return new WaitUntil(() => _isAnimationCompleted);

            _playerCollider.enabled = true;
            _playerAnimation.SetBool("Finisher", false);

            _isImpactPointReached = false;
            _isAnimationCompleted = false;
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
        private IEnumerator MoveToTarget(Transform playerTransform, Vector3 targetPosition, float stopDistance, float speed)
        {
            var distanceToTarget = Vector3.Distance(playerTransform.position, targetPosition);
            while (distanceToTarget > stopDistance)
            {
                var directionToTarget = (targetPosition - playerTransform.position).normalized;
                playerTransform.position += directionToTarget * (speed * Time.deltaTime);
                distanceToTarget = Vector3.Distance(playerTransform.position, targetPosition);
                yield return null;
            }
        }
    }
}