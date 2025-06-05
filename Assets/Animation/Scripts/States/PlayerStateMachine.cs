using Animation.Scripts.Interfaces;
using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        [SerializeField] private PlayerComponentRegistry componentRegistry;

        private PlayerState CurrentState { get; set; }

        public IPlayerAnimation PlayerAnimation => componentRegistry.PlayerAnimationInstance;
        public IPlayerMovement PlayerMovement => componentRegistry.PlayerMovementInstance;
        public IPlayerFinisher PlayerFinisher => componentRegistry.PlayerFinisherInstance;
        public IPlayerController PlayerController => componentRegistry.PlayerControllerInstance;
        public IEnemyFinishingTrigger EnemyFinishingTrigger => componentRegistry.EnemyFinishingTriggerInstance;

        private PlayerIdleState _idleState;
        private PlayerRunState _runState;
        private PlayerFinishingState _finishingState;

        public PlayerIdleState GetIdleState() => _idleState;
        public PlayerRunState GetRunState() => _runState;
        public PlayerFinishingState GetFinishingState() => _finishingState;

        private void Awake()
        {
            if (!componentRegistry)
            {
                Debug.LogError("PlayerComponentRegistry не назначен в инспекторе PlayerStateMachine. Пожалуйста, назначьте его.");
                return;
            }

            _idleState = new PlayerIdleState(this);
            _runState = new PlayerRunState(this);
            _finishingState = new PlayerFinishingState(this);
        }

        private void Start()
        {
            PlayerMovement.RotateTowardsCamera();
            ChangeState(_idleState);
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

        public void ChangeState(PlayerState newState)
        {
            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}