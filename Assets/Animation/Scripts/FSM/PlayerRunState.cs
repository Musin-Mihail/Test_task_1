using Animation.Scripts.Constants;
using Animation.Scripts.Player;

namespace Animation.Scripts.FSM
{
    public class PlayerRunState : PlayerState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly IPlayerMovement _movement;
        private readonly IPlayerRotator _rotator;
        private readonly IPlayerAnimation _animation;

        public PlayerRunState(PlayerStateMachine stateMachine, IPlayerMovement movement, IPlayerRotator rotator, IPlayerAnimation animation)
        {
            _stateMachine = stateMachine;
            _movement = movement;
            _rotator = rotator;
            _animation = animation;
        }

        public override void Enter() => _animation.SetBool(AnimationConstants.IsMoving, true);
        public override void Exit() => _animation.SetBool(AnimationConstants.IsMoving, false);

        public override void Update()
        {
            _animation.UpdateMovementAnimation(_stateMachine.CurrentMovementInput);
        }

        public override void FixedUpdate() => _movement.Move(_stateMachine.CurrentMovementInput);
        public override void LateUpdate() => _rotator.RotateToMouse();
    }
}