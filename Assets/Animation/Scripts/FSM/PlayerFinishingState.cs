using System;
using System.Collections;
using Animation.Scripts.Common;
using Animation.Scripts.Configs;
using Animation.Scripts.Constants;
using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerFinishingState : PlayerState, IDisposable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITargetMover _targetMover;
        private readonly SignalBus _signalBus;
        private readonly PlayerFinisher _playerFinisher;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerEquipment _equipment;
        private readonly IPlayerRotator _rotator;
        private readonly FinisherConfig _config;
        private readonly Transform _playerTransform;

        private bool _hasImpactPointReached;
        private bool _hasAnimationCompleted;

        public PlayerFinishingState(ICoroutineRunner coroutineRunner, ITargetMover targetMover, SignalBus signalBus, PlayerFinisher playerFinisher, IPlayerAnimation animation, IPlayerEquipment equipment, IPlayerRotator rotator, FinisherConfig config, Transform playerTransform)
        {
            _coroutineRunner = coroutineRunner;
            _targetMover = targetMover;
            _signalBus = signalBus;
            _playerFinisher = playerFinisher;
            _animation = animation;
            _equipment = equipment;
            _rotator = rotator;
            _config = config;
            _playerTransform = playerTransform;
            _signalBus.Subscribe<FinisherImpactPointReachedSignal>(() => _hasImpactPointReached = true);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(() => _hasAnimationCompleted = true);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<FinisherImpactPointReachedSignal>(() => _hasImpactPointReached = true);
            _signalBus.TryUnsubscribe<FinisherAnimationCompleteSignal>(() => _hasAnimationCompleted = true);
        }

        public override void Enter()
        {
            _hasImpactPointReached = false;
            _hasAnimationCompleted = false;

            _playerFinisher.SetFinishing(true);
            _rotator.RotateToTarget(_playerFinisher.FinisherTarget.position);
            _coroutineRunner.StartCoroutine(FinishingSequence());
        }

        private IEnumerator FinishingSequence()
        {
            // Движение к цели
            _animation.SetBool(AnimationConstants.IsMoving, true);
            yield return _targetMover.MoveToTarget(
                _playerTransform,
                _playerFinisher.FinisherTarget.position,
                _config.finishingStartDistance,
                _config.finishingMovementSpeed
            );
            _animation.SetBool(AnimationConstants.IsMoving, false);

            // Анимация добивания
            _equipment.SetWeaponActive(WeaponType.Gun, false);
            _equipment.SetWeaponActive(WeaponType.Sword, true);
            _animation.SetBool(AnimationConstants.Finisher, true);

            // Ожидание ключевых точек анимации
            yield return new WaitUntil(() => _hasImpactPointReached);
            yield return new WaitUntil(() => _hasAnimationCompleted);

            // Завершение
            _animation.SetBool(AnimationConstants.Finisher, false);
            _equipment.SetWeaponActive(WeaponType.Sword, false);
            _equipment.SetWeaponActive(WeaponType.Gun, true);

            _playerFinisher.SetFinishing(false);

            _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerIdleState) });
        }
    }
}