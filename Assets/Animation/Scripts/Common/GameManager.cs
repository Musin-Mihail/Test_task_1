using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Common
{
    /// <summary>
    /// Отвечает за общие настройки игры и жизненный цикл приложения.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Целевая частота кадров для приложения.")]
        public int targetFrameRate = 60;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
            _signalBus.Subscribe<QuitGameSignal>(() => Application.Quit());
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<QuitGameSignal>(() => Application.Quit());
        }
    }
}