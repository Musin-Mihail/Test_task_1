using System;
using System.Collections.Generic;
using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
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
        public IPlayerAnimationController PlayerAnimationController { get; private set; }

        private IPlayerEquipment _playerEquipment;
        private ITargetMover _targetMover;
        private ICoroutineRunner _coroutineRunner;
        private PlayerConfig _playerConfig;
        private Transform _playerTransform;

        [Inject]
        public void Construct(
            IPlayerAnimation playerAnimation,
            IPlayerMovement playerMovement,
            IPlayerRotator playerRotator,
            IPlayerFinisher playerFinisher,
            IPlayerController playerController,
            IEnemyFinishingTrigger enemyFinishingTrigger,
            IPlayerAnimationController playerAnimationController,
            IPlayerEquipment playerEquipment,
            ITargetMover targetMover,
            ICoroutineRunner coroutineRunner,
            PlayerConfig playerConfig,
            [Inject(Id = "PlayerTransform")] Transform playerTransform)
        {
            PlayerAnimation = playerAnimation;
            PlayerMovement = playerMovement;
            PlayerRotator = playerRotator;
            PlayerFinisher = playerFinisher;
            PlayerController = playerController;
            EnemyFinishingTrigger = enemyFinishingTrigger;
            PlayerAnimationController = playerAnimationController;

            _playerEquipment = playerEquipment;
            _targetMover = targetMover;
            _coroutineRunner = coroutineRunner;
            _playerConfig = playerConfig;
            _playerTransform = playerTransform;

            _states = new Dictionary<Type, PlayerState>();

            RegisterState(new PlayerIdleState(this));
            RegisterState(new PlayerRunState(this));
            RegisterState(new PlayerFinishingState(
                this,
                _playerEquipment,
                PlayerAnimation,
                _targetMover,
                _coroutineRunner,
                _playerConfig,
                _playerTransform,
                PlayerFinisher
            ));
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