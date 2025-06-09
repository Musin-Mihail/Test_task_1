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
    public class PlayerFinisher : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly GameObject _text;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITargetMover _targetMover;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerEquipment _equipment;
        private readonly IPlayerRotator _rotator;
        private readonly FinisherConfig _config;
        private readonly Transform _playerTransform;

        private bool _canStartFinisher;
        private Transform FinisherTarget { get; set; }
        private bool _isFinishing;
        private bool _hasImpactPointReached;
        private bool _hasAnimationCompleted;

        public PlayerFinisher(SignalBus signalBus, PlayerFacade playerFacade, ICoroutineRunner coroutineRunner, ITargetMover targetMover, IPlayerAnimation animation, IPlayerEquipment equipment, IPlayerRotator rotator, FinisherConfig config, Transform playerTransform)
        {
            _signalBus = signalBus;
            _text = playerFacade.Text;
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
            _signalBus.Subscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.Subscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
            _signalBus.Subscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
            _signalBus.Subscribe<FinisherImpactPointReachedSignal>(() => _hasImpactPointReached = true);
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(() => _hasAnimationCompleted = true);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.TryUnsubscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
            _signalBus.TryUnsubscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
            _signalBus.TryUnsubscribe<FinisherImpactPointReachedSignal>(() => _hasImpactPointReached = true);
            _signalBus.TryUnsubscribe<FinisherAnimationCompleteSignal>(() => _hasAnimationCompleted = true);
        }

        private void OnFinisherButtonPressed()
        {
            if (_canStartFinisher && !_isFinishing && FinisherTarget)
            {
                _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerFinishingState) });
            }
        }

        public void StartFinishingSequence()
        {
            if (_isFinishing) return;
            _coroutineRunner.StartCoroutine(FinishingSequenceCoroutine());
        }

        private IEnumerator FinishingSequenceCoroutine()
        {
            _isFinishing = true;
            _text.gameObject.SetActive(false);
            _hasImpactPointReached = false;
            _hasAnimationCompleted = false;

            _rotator.RotateToTarget(FinisherTarget.position);

            // Движение к цели
            _animation.SetBool(AnimationConstants.IsMoving, true);
            yield return _targetMover.MoveToTarget(
                _playerTransform,
                FinisherTarget.position,
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

            _isFinishing = false;
            FinisherTarget = null;

            _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerIdleState) });
        }


        private void OnEnemyReady(EnemyReadyForFinisherSignal signal)
        {
            if (_isFinishing) return;
            _canStartFinisher = true;
            FinisherTarget = signal.EnemyTransform;
            _text.gameObject.SetActive(true);
        }

        private void OnEnemyExited()
        {
            if (_isFinishing) return;
            _canStartFinisher = false;
            FinisherTarget = null;
            _text.gameObject.SetActive(false);
        }
    }
}