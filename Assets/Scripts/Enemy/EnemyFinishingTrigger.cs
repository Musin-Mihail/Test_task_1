using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Enemy
{
    public class EnemyFinisherTrigger : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _signalBus.Fire(new EnemyReadyForFinisherSignal { EnemyTransform = transform });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _signalBus.Fire<EnemyExitedFinisherRangeSignal>();
            }
        }
    }
}