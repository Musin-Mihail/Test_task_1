using System;

namespace Animation.Scripts.Signals
{
    public class GameStateSignals
    {
        /// <summary>
        /// // Сигнал для запроса смены состояния FSM
        /// </summary>
        public struct RequestStateChangeSignal
        {
            public Type StateType;
        }
    }
}