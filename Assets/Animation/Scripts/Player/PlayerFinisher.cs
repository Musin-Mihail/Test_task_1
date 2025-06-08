using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// </summary>
    public class PlayerFinisher : IPlayerFinisher
    {
        public event Action OnFinisherSequenceCompleted;
        public event Action OnFinisherAnimationFullyCompleted;
        public event Action OnFinisherStateReset;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private readonly PlayerConfig _playerConfig;
        private readonly IPlayerAnimation _playerAnimation;
        private readonly IPlayerEquipment _playerEquipment;
        private readonly Collider _playerCollider;
        private readonly Transform _playerTransform;
        private readonly ICoroutineRunner _coroutineRunner;
        private bool _isFinishing;

        private bool _isImpactPointReached;
        private bool _isAnimationCompleted;

        [Inject]
        public PlayerFinisher(
            Collider playerCollider,
            IPlayerAnimation playerAnimation,
            IPlayerEquipment playerEquipment,
            PlayerConfig playerConfig,
            [Inject(Id = "PlayerTransform")] Transform playerTransform,
            ICoroutineRunner coroutineRunner)
        {
            _playerCollider = playerCollider;
            _playerAnimation = playerAnimation;
            _playerEquipment = playerEquipment;
            _playerConfig = playerConfig;
            _playerTransform = playerTransform;
            _coroutineRunner = coroutineRunner;
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);
        }

        public void StartFinishingSequence()
        {
            if (!_playerConfig) return;

            _isFinishing = true;
            _playerCollider.enabled = false;
            _playerAnimation.SetBool("IsMoving", true);

            _isImpactPointReached = false;
            _isAnimationCompleted = false;

            _coroutineRunner.StartCoroutine(FinishingCoroutine());
        }

        /// <summary>
        /// Метод, вызываемый Animation Event в точке удара добивания.
        /// </summary>
        public void AnimationEvent_FinisherImpactPoint()
        {
            _isImpactPointReached = true;
            OnFinisherSequenceCompleted?.Invoke();
        }

        /// <summary>
        /// Метод, вызываемый Animation Event по завершении анимации добивания.
        /// </summary>
        public void AnimationEvent_FinisherComplete()
        {
            _isAnimationCompleted = true;
            OnFinisherAnimationFullyCompleted?.Invoke();
        }


        private IEnumerator FinishingCoroutine()
        {
            if (!_playerConfig) yield break;

            yield return _coroutineRunner.StartCoroutine(MoveToTarget(_playerTransform, TargetPosition, _playerConfig.finishingStartDistance, _playerConfig.finishingMovementSpeed));
            yield return _coroutineRunner.StartCoroutine(PerformFinishingAnimation());
            yield return _coroutineRunner.StartCoroutine(ResetFinisherState());
        }

        private IEnumerator PerformFinishingAnimation()
        {
            if (!_playerConfig) yield break;

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
            OnFinisherStateReset?.Invoke();
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