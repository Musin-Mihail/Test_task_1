using System;
using System.Collections.Generic;
using Animation.Scripts.Interfaces;
using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        [SerializeField] private PlayerComponentRegistry componentRegistry;
        private PlayerState CurrentState { get; set; }
        private Dictionary<Type, PlayerState> _states;

        public IPlayerAnimation PlayerAnimation => componentRegistry.PlayerAnimationInstance;
        public IPlayerMovement PlayerMovement => componentRegistry.PlayerMovementInstance;
        public IPlayerFinisher PlayerFinisher => componentRegistry.PlayerFinisherInstance;
        public IPlayerController PlayerController => componentRegistry.PlayerControllerInstance;
        public IEnemyFinishingTrigger EnemyFinishingTrigger => componentRegistry.EnemyFinishingTriggerInstance;

        private void Awake()
        {
            if (!componentRegistry)
            {
                Debug.LogError("PlayerComponentRegistry не назначен в инспекторе PlayerStateMachine. Пожалуйста, назначьте его.");
                return;
            }

            _states = new Dictionary<Type, PlayerState>();

            RegisterState(new PlayerIdleState(this));
            RegisterState(new PlayerRunState(this));
            RegisterState(new PlayerFinishingState(this));
        }

        private void RegisterState(PlayerState state)
        {
            _states.Add(state.GetType(), state);
        }

        private T GetState<T>() where T : PlayerState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                return (T)state;
            }

            Debug.LogError($"Состояние типа {typeof(T).Name} не найдено в машине состояний. Убедитесь, что оно существует и имеет конструктор с IPlayerStateContext.");
            return null;
        }

        private void Start()
        {
            PlayerMovement.RotateTowardsCamera();
            ChangeState<PlayerIdleState>();
        }

        private void Update()
        {
            CurrentState?.UpdateState();
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdateState();
        }

        private void LateUpdate()
        {
            CurrentState?.LateUpdateState();
        }

        private void ChangeState(PlayerState newState)
        {
            if (newState == null)
            {
                Debug.LogError("Попытка изменить состояние на null. Переход отменен.");
                return;
            }

            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        public void ChangeState<TState>() where TState : PlayerState
        {
            PlayerState newState = GetState<TState>();
            if (newState != null)
            {
                ChangeState(newState);
            }
        }
    }
}