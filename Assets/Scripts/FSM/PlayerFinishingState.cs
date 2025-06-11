using Animation.Scripts.Constants;
using Animation.Scripts.Player;
using Animation.Scripts.Signals;
using Zenject;

namespace Animation.Scripts.FSM
{
    public class PlayerFinishingState : PlayerState
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

        public override void Enter()
        {
            _signalBus.Subscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
            FinishingSequenceBeforeImpact();
        }

        public override void Exit()
        {
            _signalBus.TryUnsubscribe<FinisherAnimationCompleteSignal>(OnFinisherAnimationComplete);
        }

        private void FinishingSequenceBeforeImpact()
        {
            _equipment.SetWeaponActive(WeaponType.Gun, false);
            _equipment.SetWeaponActive(WeaponType.Sword, true);
            _animation.SetBool(AnimationConstants.Finisher, true);
        }

        private void OnFinisherAnimationComplete()
        {
            _animation.SetBool(AnimationConstants.Finisher, false);
            _equipment.SetWeaponActive(WeaponType.Sword, false);
            _equipment.SetWeaponActive(WeaponType.Gun, true);

            _availabilityService.SetFinisherInProgress(false);
            StateMachine.IsFinisherRequested = false;
        }
    }
}