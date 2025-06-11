using Animation.Scripts.Player;

namespace Animation.Scripts.FSM
{
    public class PlayerIdleState : PlayerState
    {
        private readonly IPlayerRotator _rotator;

        public PlayerIdleState(IPlayerRotator rotator)
        {
            _rotator = rotator;
        }

        public override void Enter()
        {
            _rotator.RotateTowardsCamera();
            StateMachine.IsFinisherRequested = false;
        }

        public override void LateUpdate()
        {
            _rotator.RotateToMouse();
        }
    }
}