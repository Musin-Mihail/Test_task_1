using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.States
{
    public class PlayerStateMachine : MonoBehaviour, IPlayerStateContext
    {
        private PlayerState CurrentState { get; set; }
        private StateFactory _stateFactory;
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
            StateFactory stateFactory)
        {
            PlayerAnimation = playerAnimation;
            PlayerMovement = playerMovement;
            PlayerRotator = playerRotator;
            PlayerFinisher = playerFinisher;
            PlayerController = playerController;
            EnemyFinishingTrigger = enemyFinishingTrigger;
            PlayerAnimationController = playerAnimationController;
            _stateFactory = stateFactory;
        }

        private void Start()
        {
            ChangeState<PlayerIdleState>();
        }

        private void Update() => CurrentState?.UpdateState();
        private void FixedUpdate() => CurrentState?.FixedUpdateState();
        private void LateUpdate() => CurrentState?.LateUpdateState();

        public void ChangeState<TState>() where TState : PlayerState
        {
            CurrentState?.ExitState();

            PlayerState newState = _stateFactory.CreateState<TState>();
            newState.Initialize(this);

            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}