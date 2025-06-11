using Animation.Scripts.Constants;
using Animation.Scripts.Player;

namespace Animation.Scripts.FSM
{
    public class PlayerRunState : PlayerState
    {
        private readonly IPlayerMovement _movement;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerRotator _rotator;

        public PlayerRunState(IPlayerMovement movement, IPlayerAnimation animation, IPlayerRotator rotator)
        {
            _movement = movement;
            _animation = animation;
            _rotator = rotator;
        }

        public override void Enter() => _animation.SetBool(AnimationConstants.IsMoving, true);
        public override void Exit() => _animation.SetBool(AnimationConstants.IsMoving, false);
        public override void Update() => _animation.UpdateMovementAnimation(StateMachine.CurrentMovementInput);
        public override void FixedUpdate() => _movement.Move(StateMachine.CurrentMovementInput);

        public override void LateUpdate()
        {
            _rotator.RotateToMouse();
        }
    }
}