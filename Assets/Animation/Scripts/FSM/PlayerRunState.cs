using Animation.Scripts.Constants;
using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerRunState : PlayerState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly IPlayerMovement _movement;
        private readonly IPlayerRotator _rotator;
        private readonly IPlayerAnimation _animation;
        private readonly SignalBus _signalBus;

        public PlayerRunState(PlayerStateMachine stateMachine, IPlayerMovement movement, IPlayerRotator rotator, IPlayerAnimation animation, SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _movement = movement;
            _rotator = rotator;
            _animation = animation;
            _signalBus = signalBus;
        }

        public override void Enter()
        {
            _animation.SetBool(AnimationConstants.IsMoving, true);
        }

        public override void Exit()
        {
            _animation.SetBool(AnimationConstants.IsMoving, false);
        }

        public override void Update()
        {
            if (!_stateMachine.IsMoving)
            {
                _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerIdleState) });
                return;
            }

            _animation.UpdateMovementAnimation(_stateMachine.CurrentMovementInput);
        }

        public override void FixedUpdate()
        {
            _movement.Move(_stateMachine.CurrentMovementInput);
        }

        public override void LateUpdate()
        {
            _rotator.RotateToMouse();
        }
    }
}