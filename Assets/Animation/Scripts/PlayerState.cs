namespace Animation.Scripts
{
    public abstract class PlayerState
    {
        protected readonly PlayerStateMachine PlayerStateMachine;

        protected PlayerState(PlayerStateMachine playerStateMachine)
        {
            PlayerStateMachine = playerStateMachine;
        }

        public virtual void EnterState()
        {
        }

        public virtual void ExitState()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void FixedUpdateState()
        {
        }

        public virtual void LateUpdateState()
        {
        }
    }
}