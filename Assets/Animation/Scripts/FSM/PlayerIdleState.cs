using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerIdleState : PlayerState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly IPlayerRotator _rotator;
        private readonly SignalBus _signalBus;

        public PlayerIdleState(PlayerStateMachine stateMachine, IPlayerRotator rotator, SignalBus signalBus)
        {
            _stateMachine = stateMachine;
            _rotator = rotator;
            _signalBus = signalBus;
        }

        public override void Enter()
        {
            _rotator.RotateTowardsCamera();
        }

        public override void Update()
        {
            if (_stateMachine.IsMoving)
            {
                _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerRunState) });
            }
        }

        public override void LateUpdate()
        {
            _rotator.RotateToMouse();
        }
    }
}