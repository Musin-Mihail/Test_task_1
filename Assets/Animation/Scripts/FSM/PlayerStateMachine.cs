using System;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerStateMachine : IInitializable, ITickable, IFixedTickable, ILateTickable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly StateFactory _stateFactory;

        private PlayerState _currentState;
        public Vector3 CurrentMovementInput { get; private set; }
        public bool IsMoving => CurrentMovementInput.magnitude > 0.01f;

        public PlayerStateMachine(SignalBus signalBus, StateFactory stateFactory)
        {
            _signalBus = signalBus;
            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<MovementInputSignal>(OnMovementInput);
            _signalBus.Subscribe<GameStateSignals.RequestStateChangeSignal>(OnChangeStateRequested);
            _signalBus.Subscribe<QuitGameSignal>(() => Application.Quit());

            ChangeState(typeof(PlayerIdleState));
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<MovementInputSignal>(OnMovementInput);
            _signalBus.Unsubscribe<GameStateSignals.RequestStateChangeSignal>(OnChangeStateRequested);
        }

        private void OnMovementInput(MovementInputSignal signal)
        {
            CurrentMovementInput = new Vector3(signal.InputValue.x, 0, signal.InputValue.y).normalized;
        }

        private void OnChangeStateRequested(GameStateSignals.RequestStateChangeSignal signal)
        {
            ChangeState(signal.StateType);
        }

        private void ChangeState(Type newStateType)
        {
            _currentState?.Exit();
            _currentState = _stateFactory.Create(newStateType);
            _currentState.Enter();
        }

        public void Tick() => _currentState?.Update();
        public void FixedTick() => _currentState?.FixedUpdate();
        public void LateTick() => _currentState?.LateUpdate();
    }
}