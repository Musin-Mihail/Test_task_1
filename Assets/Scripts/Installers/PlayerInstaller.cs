using Animation.Scripts.FSM;
using Animation.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerFacade playerFacade;

        public override void InstallBindings()
        {
            Container.Bind<PlayerFacade>().FromInstance(playerFacade).AsSingle();
            Container.Bind<Transform>().FromInstance(playerFacade.transform).AsSingle();
            Container.Bind<Animator>().FromInstance(playerFacade.Animator).AsSingle();
            Container.Bind<Transform>().WithId("ChestTransform").FromInstance(playerFacade.ChestTransform).AsCached();

            Container.Bind<IPlayerInput>().To<PlayerInput>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerEquipment>().To<PlayerEquipment>().FromInstance(playerFacade.PlayerEquipment).AsSingle();
            Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnimationEventBridge>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FinisherAvailabilityService>().AsSingle().NonLazy();

            InstallFSM();
        }

        private void InstallFSM()
        {
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
            Container.Bind<StateFactory>().AsSingle();

            Container.Bind<PlayerIdleState>().AsTransient();
            Container.Bind<PlayerRunState>().AsTransient();
            Container.Bind<PlayerApproachingState>().AsTransient();
            Container.Bind<PlayerFinishingState>().AsTransient();
        }
    }
}