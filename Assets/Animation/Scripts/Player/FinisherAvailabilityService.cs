using System;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class FinisherAvailabilityService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly GameObject _finisherPromptText;

        public bool IsFinisherAvailable { get; private set; }
        public Transform FinisherTarget { get; private set; }

        public FinisherAvailabilityService(SignalBus signalBus, PlayerFacade playerFacade)
        {
            _signalBus = signalBus;
            _finisherPromptText = playerFacade.Text;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.Subscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
            _finisherPromptText.SetActive(false);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyReadyForFinisherSignal>(OnEnemyReady);
            _signalBus.Unsubscribe<EnemyExitedFinisherRangeSignal>(OnEnemyExited);
        }

        public void SetFinisherInProgress(bool inProgress)
        {
            if (inProgress)
            {
                IsFinisherAvailable = false;
                _finisherPromptText.SetActive(false);
            }
        }

        private void OnEnemyReady(EnemyReadyForFinisherSignal signal)
        {
            IsFinisherAvailable = true;
            FinisherTarget = signal.EnemyTransform;
            _finisherPromptText.SetActive(true);
        }

        private void OnEnemyExited()
        {
            IsFinisherAvailable = false;

            _finisherPromptText.SetActive(false);
        }
    }
}