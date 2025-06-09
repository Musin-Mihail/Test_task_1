using System;
using System.Collections.Generic;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerStateMachine : IInitializable, ITickable, IFixedTickable, ILateTickable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly StateFactory _stateFactory;
        private readonly List<Transition> _transitions = new();

        private PlayerState _currentState;
        public Vector3 CurrentMovementInput { get; private set; }
        private bool IsMoving => CurrentMovementInput.magnitude > 0.01f;

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

            CreateTransitions();

            ChangeState(typeof(PlayerIdleState));
        }

        private void CreateTransitions()
        {
            AddTransition(typeof(PlayerIdleState), typeof(PlayerRunState), () => IsMoving);
            AddTransition(typeof(PlayerRunState), typeof(PlayerIdleState), () => !IsMoving);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<MovementInputSignal>(OnMovementInput);
            _signalBus.Unsubscribe<GameStateSignals.RequestStateChangeSignal>(OnChangeStateRequested);
        }

        private void CheckForTransition()
        {
            foreach (var transition in _transitions)
            {
                if (transition.From == _currentState.GetType() && transition.Condition())
                {
                    ChangeState(transition.To);
                    return;
                }
            }
        }

        private void ChangeState(Type newStateType)
        {
            if (_currentState?.GetType() == newStateType) return;

            _currentState?.Exit();
            _currentState = _stateFactory.Create(newStateType);
            _currentState.Enter();
        }

        private void AddTransition(Type from, Type to, Func<bool> condition)
        {
            var transition = new Transition(from, to, condition);
            _transitions.Add(transition);
        }

        private void OnMovementInput(MovementInputSignal signal) =>
            CurrentMovementInput = new Vector3(signal.InputValue.x, 0, signal.InputValue.y).normalized;

        private void OnChangeStateRequested(GameStateSignals.RequestStateChangeSignal signal) =>
            ChangeState(signal.StateType);

        public void Tick()
        {
            CheckForTransition();
            _currentState?.Update();
        }

        public void FixedTick() => _currentState?.FixedUpdate();
        public void LateTick() => _currentState?.LateUpdate();
    }

    public class Transition
    {
        public Type From { get; }
        public Type To { get; }
        public Func<bool> Condition { get; }

        public Transition(Type from, Type to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }
    }
}