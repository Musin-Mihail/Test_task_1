using System;
using System.Collections;
using Animation.Scripts.Common;
using Animation.Scripts.Configs;
using Animation.Scripts.Constants;
using Animation.Scripts.FSM;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class FinisherExecutionService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly FinisherAvailabilityService _availabilityService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITargetMover _targetMover;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerEquipment _equipment;
        private readonly IPlayerRotator _rotator;
        private readonly FinisherConfig _config;
        private readonly Transform _playerTransform;

        private bool _isFinishing;
        private bool _hasImpactPointReached;
        private bool _hasAnimationCompleted;

        public FinisherExecutionService(
            SignalBus signalBus,
            FinisherAvailabilityService availabilityService,
            ICoroutineRunner coroutineRunner,
            ITargetMover targetMover,
            IPlayerAnimation animation,
            IPlayerEquipment equipment,
            IPlayerRotator rotator,
            FinisherConfig config,
            Transform playerTransform)
        {
            _signalBus = signalBus;
            _availabilityService = availabilityService;
            _coroutineRunner = coroutineRunner;
            _targetMover = targetMover;
            _animation = animation;
            _equipment = equipment;
            _rotator = rotator;
            _config = config;
            _playerTransform = playerTransform;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
            _signalBus.Subscribe<FinisherImpactPointReachedSignal>(OnFinisherImpact);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
            _signalBus.Unsubscribe<FinisherImpactPointReachedSignal>(OnFinisherImpact);
            _signalBus.Unsubscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
        }
        
        private void OnFinisherImpact() => _hasImpactPointReached = true;
        private void OnFinisherAnimationComplete() => _hasAnimationCompleted = true;

        private void OnFinisherButtonPressed()
        {
            if (_availabilityService.IsFinisherAvailable && !_isFinishing)
            {
                _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerFinishingState) });
                StartFinishingSequence();
            }
        }

        private void StartFinishingSequence()
        {
            if (_isFinishing) return;
            _coroutineRunner.StartCoroutine(FinishingSequenceCoroutine());
        }

        private IEnumerator FinishingSequenceCoroutine()
        {
            _isFinishing = true;
            _availabilityService.SetFinisherInProgress(true);
            _hasImpactPointReached = false;
            _hasAnimationCompleted = false;

            _rotator.RotateToTarget(_availabilityService.FinisherTarget.position);

            _animation.SetBool(AnimationConstants.IsMoving, true);
            yield return _targetMover.MoveToTarget(
                _playerTransform,
                _availabilityService.FinisherTarget.position,
                _config.finishingStartDistance,
                _config.finishingMovementSpeed
            );
            _animation.SetBool(AnimationConstants.IsMoving, false);

            _equipment.SetWeaponActive(WeaponType.Gun, false);
            _equipment.SetWeaponActive(WeaponType.Sword, true);
            _animation.SetBool(AnimationConstants.Finisher, true);

            yield return new WaitUntil(() => _hasImpactPointReached);
            yield return new WaitUntil(() => _hasAnimationCompleted);

            _animation.SetBool(AnimationConstants.Finisher, false);
            _equipment.SetWeaponActive(WeaponType.Sword, false);
            _equipment.SetWeaponActive(WeaponType.Gun, true);
            
            _isFinishing = false;
            
            _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerIdleState) });
        }
    }
}