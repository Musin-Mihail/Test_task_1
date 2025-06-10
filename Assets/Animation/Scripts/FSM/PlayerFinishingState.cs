using System;
using Animation.Scripts.Constants;
using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerFinishingState : PlayerState, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerEquipment _equipment;
        private readonly FinisherAvailabilityService _availabilityService;

        public PlayerFinishingState(
            SignalBus signalBus,
            IPlayerAnimation animation,
            IPlayerEquipment equipment,
            FinisherAvailabilityService availabilityService)
        {
            _signalBus = signalBus;
            _animation = animation;
            _equipment = equipment;
            _availabilityService = availabilityService;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<FinisherAnimationCompleteSignal>(FinishingSequenceAfterImpact);
        }

        public override void Enter()
        {
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(FinishingSequenceAfterImpact);
            FinishingSequenceBeforeImpact();
        }

        public override void Exit()
        {
            Dispose();
        }

        private void FinishingSequenceBeforeImpact()
        {
            _equipment.SetWeaponActive(WeaponType.Gun, false);
            _equipment.SetWeaponActive(WeaponType.Sword, true);
            _animation.SetBool(AnimationConstants.Finisher, true);
        }

        private void FinishingSequenceAfterImpact()
        {
            _animation.SetBool(AnimationConstants.Finisher, false);
            _equipment.SetWeaponActive(WeaponType.Sword, false);
            _equipment.SetWeaponActive(WeaponType.Gun, true);
            _availabilityService.SetFinisherInProgress(false);
            StateMachine.IsFinisherRequested = false;
        }
    }
}