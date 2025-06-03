namespace Animation.Scripts.States
{
    public abstract class PlayerState
    {
        protected readonly IPlayerStateContext Context;

        protected PlayerState(IPlayerStateContext context)
        {
            Context = context;
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