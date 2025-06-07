using System;
using System.Collections.Generic;
using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        private PlayerState CurrentState { get; set; }
        private Dictionary<Type, PlayerState> _states;

        public IPlayerAnimation PlayerAnimation { get; private set; }

        public IPlayerMovement PlayerMovement { get; private set; }

        public IPlayerRotator PlayerRotator { get; private set; }

        public IPlayerFinisher PlayerFinisher { get; private set; }

        public IPlayerController PlayerController { get; private set; }

        public IEnemyFinishingTrigger EnemyFinishingTrigger { get; private set; }

        [Inject]
        public void Construct(
            IPlayerAnimation playerAnimation,
            IPlayerMovement playerMovement,
            IPlayerRotator playerRotator,
            IPlayerFinisher playerFinisher,
            IPlayerController playerController,
            IEnemyFinishingTrigger enemyFinishingTrigger)
        {
            PlayerAnimation = playerAnimation;
            PlayerMovement = playerMovement;
            PlayerRotator = playerRotator;
            PlayerFinisher = playerFinisher;
            PlayerController = playerController;
            EnemyFinishingTrigger = enemyFinishingTrigger;

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
            PlayerRotator.RotateTowardsCamera();
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