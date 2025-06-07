using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var assembly = Assembly.GetAssembly(typeof(PlayerStateMachine));
            var playerStateTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(PlayerState)));

            foreach (var stateType in playerStateTypes)
            {
                try
                {
                    var constructor = stateType.GetConstructor(new[] { typeof(IPlayerStateContext) });
                    if (constructor != null)
                    {
                        var stateInstance = (PlayerState)constructor.Invoke(new object[] { this });
                        RegisterState(stateInstance);
                    }
                    else
                    {
                        Debug.LogWarning($"Состояние типа {stateType.Name} не имеет конструктора, принимающего IPlayerStateContext. Оно не будет зарегистрировано.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка при регистрации состояния типа {stateType.Name}: {e.Message}");
                }
            }

            if (_states.Count == 0)
            {
                Debug.LogWarning("Не найдено и не зарегистрировано ни одного состояния. Убедитесь, что PlayerState-классы существуют и имеют правильные конструкторы.");
            }
            else
            {
                Debug.Log($"Зарегистрировано состояний: {_states.Count}.");
                foreach (var stateEntry in _states)
                {
                    Debug.Log($" - {stateEntry.Key.Name}");
                }
            }
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