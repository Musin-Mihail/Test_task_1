using Animation.Scripts.Interfaces;
using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerFinisher playerFinisher;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private EnemyFinishingTrigger enemyFinishingTrigger;
        private PlayerState CurrentState { get; set; }
        public PlayerAnimation PlayerAnimation => playerAnimation;
        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerFinisher PlayerFinisher => playerFinisher;
        public PlayerController PlayerController => playerController;
        public EnemyFinishingTrigger EnemyFinishingTrigger => enemyFinishingTrigger;

        private PlayerIdleState _idleState;
        private PlayerRunState _runState;
        private PlayerFinishingState _finishingState;

        public PlayerIdleState GetIdleState() => _idleState;
        public PlayerRunState GetRunState() => _runState;
        public PlayerFinishingState GetFinishingState() => _finishingState;

        private void Awake()
        {
            _idleState = new PlayerIdleState(this);
            _runState = new PlayerRunState(this);
            _finishingState = new PlayerFinishingState(this);
            if (playerMovement)
            {
                playerMovement.Initialize(playerController, playerFinisher);
            }
            else
            {
                Debug.LogError("PlayerMovement не назначен в инспекторе PlayerStateMachine.");
            }

            if (playerFinisher)
            {
                var playerCollider = GetComponent<Collider>();
                if (playerCollider)
                {
                    playerFinisher.Initialize(playerCollider, playerAnimation, playerMovement);
                }
                else
                {
                    Debug.LogError("Collider не найден на GameObject PlayerStateMachine.");
                }
            }
            else
            {
                Debug.LogError("PlayerFinisher не назначен в инспекторе PlayerStateMachine.");
            }

            if (enemyFinishingTrigger)
            {
                enemyFinishingTrigger.Initialize(playerFinisher);
            }
            else
            {
                Debug.LogError("EnemyFinishingTrigger не назначен в инспекторе PlayerStateMachine.");
            }
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