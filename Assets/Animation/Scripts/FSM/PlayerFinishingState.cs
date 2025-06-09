using Animation.Scripts.Player;

namespace Animation.Scripts.FSM
{
    public class PlayerFinishingState : PlayerState
    {
        private readonly PlayerFinisher _playerFinisher;

        public PlayerFinishingState(PlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
        }

        public override void Enter()
        {
            _playerFinisher.StartFinishingSequence();
        }
    }
}