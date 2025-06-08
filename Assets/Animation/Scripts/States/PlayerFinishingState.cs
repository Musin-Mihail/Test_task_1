using System;
using System.Collections;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.States
{
    public class PlayerFinishingState : PlayerState, IDisposable
    {
        private readonly IPlayerEquipment _playerEquipment;
        private readonly IPlayerAnimation _playerAnimation;
        private readonly ITargetMover _targetMover;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly FinisherConfig _finisherConfig;
        private readonly Transform _playerTransform;
        private readonly IPlayerFinisher _playerFinisher;
        private readonly SignalBus _signalBus;

        private bool _hasImpactPointReached;
        private bool _hasAnimationFullyCompleted;

        [Inject]
        public PlayerFinishingState(
            IPlayerEquipment playerEquipment,
            IPlayerAnimation playerAnimation,
            ITargetMover targetMover,
            ICoroutineRunner coroutineRunner,
            FinisherConfig finisherConfig,
            [Inject(Id = "PlayerTransform")] Transform playerTransform,
            IPlayerFinisher playerFinisher,
            SignalBus signalBus)
        {
            _playerEquipment = playerEquipment;
            _playerAnimation = playerAnimation;
            _targetMover = targetMover;
            _coroutineRunner = coroutineRunner;
            _finisherConfig = finisherConfig;
            _playerTransform = playerTransform;
            _playerFinisher = playerFinisher;
            _signalBus = signalBus;
        }

        public override void EnterState()
        {
            _signalBus.Subscribe<FinisherImpactSignal>(HandleFinisherImpactPoint);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(HandleFinisherAnimationComplete);
            _playerFinisher.OnFinisherStateReset += ResetFinisherStateFlags;

            _hasImpactPointReached = false;
            _hasAnimationFullyCompleted = false;

            Context.PlayerRotator.RotationToTarget(Context.PlayerFinisher.TargetPosition);
            _playerFinisher.SetFinishing(true);
            _coroutineRunner.StartCoroutine(FinishingSequenceCoroutine());
        }

        public override void ExitState()
        {
            _signalBus.Unsubscribe<FinisherImpactSignal>(HandleFinisherImpactPoint);
            _signalBus.Unsubscribe<FinisherAnimationCompleteSignal>(HandleFinisherAnimationComplete);
            _playerFinisher.OnFinisherStateReset -= ResetFinisherStateFlags;
        }

        public void Dispose()
        {
            ExitState();
        }

        public override void UpdateState()
        {
            if (!_playerFinisher.IsFinishing())
            {
                Context.ChangeState<PlayerIdleState>();
            }
        }

        private IEnumerator FinishingSequenceCoroutine()
        {
            _playerAnimation.SetBool("IsMoving", true);
            yield return _coroutineRunner.StartCoroutine(_targetMover.MoveToTarget(_playerTransform, Context.PlayerFinisher.TargetPosition, _finisherConfig.finishingStartDistance, _finisherConfig.finishingMovementSpeed));

            _playerAnimation.SetBool("IsMoving", false);
            _playerEquipment.SetWeaponActive(WeaponType.Gun, false);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, true);
            _playerAnimation.SetBool("Finisher", true);

            yield return new WaitUntil(() => _hasImpactPointReached);
            yield return new WaitUntil(() => _hasAnimationFullyCompleted);

            _playerAnimation.SetBool("Finisher", false);
            _playerEquipment.SetWeaponActive(WeaponType.Gun, true);
            _playerEquipment.SetWeaponActive(WeaponType.Sword, false);
            _playerFinisher.SetFinishing(false);
            _playerFinisher.ResetFinisherState();
        }

        private void HandleFinisherImpactPoint()
        {
            _hasImpactPointReached = true;
        }

        private void HandleFinisherAnimationComplete()
        {
            _hasAnimationFullyCompleted = true;
        }

        private void ResetFinisherStateFlags()
        {
            _hasImpactPointReached = false;
            _hasAnimationFullyCompleted = false;
        }
    }
}