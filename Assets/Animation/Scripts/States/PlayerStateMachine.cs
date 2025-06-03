using UnityEngine;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        private PlayerState CurrentState { get; set; }
        public PlayerAnimation PlayerAnimation { get; private set; }
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerFinisher PlayerFinisher { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public EnemyFinishingTrigger EnemyFinishingTrigger { get; private set; }

        private PlayerIdleState _idleState;
        private PlayerRunState _runState;
        private PlayerFinishingState _finishingState;

        public PlayerIdleState GetIdleState() => _idleState;
        public PlayerRunState GetRunState() => _runState;
        public PlayerFinishingState GetFinishingState() => _finishingState;

        private void Awake()
        {
            PlayerAnimation = GetComponent<PlayerAnimation>();
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerFinisher = GetComponent<PlayerFinisher>();
            PlayerController = GetComponent<PlayerController>();
            EnemyFinishingTrigger = GetComponent<EnemyFinishingTrigger>();

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