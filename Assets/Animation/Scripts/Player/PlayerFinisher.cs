using System;
using Animation.Scripts.FSM;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class PlayerFinisher : IDisposable
    {
        private bool CanStartFinisher { get; set; }
        public Transform FinisherTarget { get; private set; }
        private bool IsFinishing { get; set; }

        private readonly SignalBus _signalBus;
        private readonly GameObject _text;

        public PlayerFinisher(SignalBus signalBus, [Inject] PlayerFacade playerFacade)
        {
            _signalBus = signalBus;
            _text = playerFacade.Text;

            _signalBus.Subscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.Subscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
            _signalBus.Subscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.Unsubscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
            _signalBus.Unsubscribe<FinisherButtonSignal>(OnFinisherButtonPressed);
        }

        public void SetFinishing(bool isFinishing)
        {
            IsFinishing = isFinishing;
            _text.gameObject.SetActive(false);
            if (!isFinishing)
            {
                FinisherTarget = null;
            }
        }

        private void OnEnemyReady(EnemyReadyForFinisherSignal signal)
        {
            CanStartFinisher = true;
            FinisherTarget = signal.EnemyTransform;
            _text.gameObject.SetActive(true);
        }

        private void OnEnemyExited()
        {
            CanStartFinisher = false;
            FinisherTarget = null;
            _text.gameObject.SetActive(false);
        }

        private void OnFinisherButtonPressed()
        {
            if (CanStartFinisher && !IsFinishing && FinisherTarget)
            {
                _signalBus.Fire(new GameStateSignals.RequestStateChangeSignal { StateType = typeof(PlayerFinishingState) });
            }
        }
    }
}