using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за логику добивания игрока.
    /// Реализует интерфейс IPlayerFinisher.
    /// </summary>
    public class PlayerFinisher : MonoBehaviour, IPlayerFinisher
    {
        [SerializeField, Tooltip("Дистанция до цели для начала добивания")]
        private float finishingStartDistance = 2.5f;

        [SerializeField, Tooltip("Время анимации до удара")]
        private float timeBeforeImpact = 0.4f;

        [SerializeField, Tooltip("Время на выполнение анимации удара")]
        private float finishingStrikeDuration = 1.2f;

        public event Action OnFinisherSequenceCompleted;

        public Vector3 TargetPosition { get; set; }
        public bool IsFinishing() => _isFinishing;

        private IPlayerAnimation _playerAnimation;
        private IPlayerMovement _playerMovement;
        private IPlayerEquipment _playerEquipment;
        private Collider _playerCollider;
        private bool _isFinishing;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerCollider">Ссылка на Collider игрока.</param>
        /// <param name="playerAnimation">Ссылка на IPlayerAnimation.</param>
        /// <param name="playerMovement">Ссылка на IPlayerMovement.</param>
        /// <param name="playerEquipment">Ссылка на IPlayerEquipment.</param>
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
            yield return StartCoroutine(_playerMovement.MoveToTarget(TargetPosition, finishingStartDistance));
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
    }
}