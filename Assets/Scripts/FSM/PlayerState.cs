using System;
using System.Collections.Generic;

namespace Animation.Scripts.FSM
{
    public abstract class PlayerState
    {
        protected PlayerStateMachine StateMachine;
        private readonly List<Transition> _transitions = new();

        public void Initialize(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void AddTransition(Func<bool> condition, PlayerState targetState)
        {
            _transitions.Add(new Transition(condition, targetState));
        }

        public PlayerState GetNextState()
        {
            foreach (var transition in _transitions)
            {
                if (transition.Condition())
                {
                    return transition.To;
                }
            }

            return this;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void LateUpdate()
        {
        }
    }

    public class Transition
    {
        public Func<bool> Condition { get; }
        public PlayerState To { get; }

        public Transition(Func<bool> condition, PlayerState to)
        {
            Condition = condition;
            To = to;
        }
    }
}