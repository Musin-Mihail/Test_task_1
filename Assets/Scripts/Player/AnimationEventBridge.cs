using Animation.Scripts.Constants;
using Animation.Scripts.Signals;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class AnimationEventBridge
    {
        private readonly SignalBus _signalBus;

        public AnimationEventBridge(SignalBus signalBus, PlayerFacade playerFacade)
        {
            _signalBus = signalBus;
            var bridgeComponent = playerFacade.Animator.GetComponent<AnimationEventReceiver>();
            if (!bridgeComponent)
            {
                bridgeComponent = playerFacade.Animator.gameObject.AddComponent<AnimationEventReceiver>();
            }

            bridgeComponent.OnAnimationEvent += HandleAnimationEvent;
        }

        private void HandleAnimationEvent(string eventName)
        {
            switch (eventName)
            {
                case EventNames.FinisherImpactPoint:
                    _signalBus.Fire<FinisherImpactPointReachedSignal>();
                    break;
                case EventNames.FinisherComplete:
                    _signalBus.Fire<FinisherAnimationCompleteSignal>();
                    break;
                default:
                    Debug.LogWarning($"Unhandled animation event: {eventName}");
                    break;
            }
        }
    }
}