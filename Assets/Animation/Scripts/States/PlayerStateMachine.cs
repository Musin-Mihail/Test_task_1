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
        private DiContainer _container;

        public IPlayerAnimation PlayerAnimation { get; private set; }
        public IPlayerMovement PlayerMovement { get; private set; }
        public IPlayerRotator PlayerRotator { get; private set; }
        public IPlayerFinisher PlayerFinisher { get; private set; }
        public IPlayerController PlayerController { get; private set; }
        public IEnemyFinishingTrigger EnemyFinishingTrigger { get; private set; }
        public IPlayerAnimationController PlayerAnimationController { get; private set; }

        [Inject]
        public void Construct(
            IPlayerAnimation playerAnimation,
            IPlayerMovement playerMovement,
            IPlayerRotator playerRotator,
            IPlayerFinisher playerFinisher,
            IPlayerController playerController,
            IEnemyFinishingTrigger enemyFinishingTrigger,
            IPlayerAnimationController playerAnimationController,
            DiContainer container)
        {
            PlayerAnimation = playerAnimation;
            PlayerMovement = playerMovement;
            PlayerRotator = playerRotator;
            PlayerFinisher = playerFinisher;
            PlayerController = playerController;
            EnemyFinishingTrigger = enemyFinishingTrigger;
            PlayerAnimationController = playerAnimationController;
            _container = container;

            _states = new Dictionary<Type, PlayerState>();

            RegisterState(_container.Resolve<PlayerIdleState>());
            RegisterState(_container.Resolve<PlayerRunState>());
            RegisterState(_container.Resolve<PlayerFinishingState>());
        }

        private void RegisterState<TState>(TState state) where TState : PlayerState
        {
            state.Initialize(this);
            _states.Add(typeof(TState), state);
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