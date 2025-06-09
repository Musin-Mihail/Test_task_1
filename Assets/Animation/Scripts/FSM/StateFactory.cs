using System;
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

        public PlayerState Create(Type stateType)
        {
            return _container.Instantiate(stateType) as PlayerState;
        }
    }
}