using Zenject;

namespace Animation.Scripts.FSM
{
    public class StateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container)
        {
            _container = container;
        }

        public T Create<T>() where T : PlayerState
        {
            var state = _container.Instantiate<T>();
            state.Initialize(_container.Resolve<PlayerStateMachine>());
            return state;
        }
    }
}