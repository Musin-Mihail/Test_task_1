using Animation.Scripts.Interfaces;
using Zenject;

namespace Animation.Scripts.States
{
    public abstract class PlayerState
    {
        protected IPlayerStateContext Context;

        [Inject]
        public PlayerState()
        {
        }

        public void Initialize(IPlayerStateContext context)
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