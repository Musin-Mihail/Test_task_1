using System;
using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerStateMachine : IInitializable, ITickable, IFixedTickable, ILateTickable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly StateFactory _stateFactory;
        private readonly IPlayerRotator _rotator;

        private PlayerState _currentState;

        public Vector3 CurrentMovementInput { get; private set; }
        public bool IsFinisherRequested { get; set; }
        public bool IsTargetReached { get; set; }
        public bool IsMoving => CurrentMovementInput.magnitude > 0.01f;

        public PlayerStateMachine(SignalBus signalBus, StateFactory stateFactory, IPlayerRotator rotator)
        {
            _signalBus = signalBus;
            _stateFactory = stateFactory;
            _rotator = rotator;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<MovementInputSignal>(OnMovementInput);
            _signalBus.Subscribe<FinisherButtonSignal>(OnFinisherButton);

            SetupStates();
        }

        private void SetupStates()
        {
            var idleState = _stateFactory.Create<PlayerIdleState>();
            var runState = _stateFactory.Create<PlayerRunState>();
            var approachingState = _stateFactory.Create<PlayerApproachingState>();
            var finishingState = _stateFactory.Create<PlayerFinishingState>();

            idleState.AddTransition(() => IsMoving, runState);
            idleState.AddTransition(() => IsFinisherRequested, approachingState);

            runState.AddTransition(() => !IsMoving, idleState);
            runState.AddTransition(() => IsFinisherRequested, approachingState);

            approachingState.AddTransition(() => IsTargetReached, finishingState);

            finishingState.AddTransition(() => !IsFinisherRequested, idleState);

            ChangeState(idleState);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<MovementInputSignal>(OnMovementInput);
            _signalBus.TryUnsubscribe<FinisherButtonSignal>(OnFinisherButton);
        }

        public void Tick()
        {
            var nextState = _currentState.GetNextState();
            if (nextState != _currentState)
            {
                ChangeState(nextState);
            }

            _currentState?.Update();
        }

        public void FixedTick() => _currentState?.FixedUpdate();

        public void LateTick()
        {
            _currentState?.LateUpdate();
        }

        private void ChangeState(PlayerState newState)
        {
            if (_currentState == newState) return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void OnMovementInput(MovementInputSignal signal) => CurrentMovementInput = new Vector3(signal.InputValue.x, 0, signal.InputValue.y).normalized;
        private void OnFinisherButton() => IsFinisherRequested = true;
    }
}