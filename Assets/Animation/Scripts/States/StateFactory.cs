using Zenject;

namespace Animation.Scripts.States
{
    /// <summary>
    /// Фабрика для создания экземпляров состояний с внедрением зависимостей.
    /// </summary>
    public class StateFactory
    {
        private readonly DiContainer _container;

        [Inject]
        public StateFactory(DiContainer container)
        {
            _container = container;
        }

        public TState CreateState<TState>() where TState : PlayerState
        {
            return _container.Instantiate<TState>();
        }
    }
}