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
            // --- FACADE & COMPONENTS ---
            Container.Bind<PlayerFacade>().FromInstance(playerFacade).AsSingle();
            Container.Bind<Transform>().FromInstance(playerFacade.transform).AsSingle();
            Container.Bind<Animator>().FromInstance(playerFacade.Animator).AsSingle();

            Container.Bind<Transform>().WithId("ChestTransform").FromInstance(playerFacade.ChestTransform).AsCached();

            // --- MONOBEHAVIOURS ---
            Container.Bind<IPlayerInput>().To<PlayerInput>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerEquipment>().To<PlayerEquipment>().FromInstance(playerFacade.PlayerEquipment).AsSingle();
            Container.Bind<ICameraController>().To<CameraController>().FromComponentInHierarchy().AsSingle();

            // --- POCO SERVICES ---
            Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnimationEventBridge>().AsSingle().NonLazy();

            // --- НОВЫЕ СЕРВИСЫ ДЛЯ ДОБИВАНИЯ ---
            Container.BindInterfacesAndSelfTo<FinisherAvailabilityService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FinisherExecutionService>().AsSingle().NonLazy();

            // --- STATE MACHINE ---
            InstallFSM();
        }

        private void InstallFSM()
        {
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle().NonLazy();
            Container.Bind<StateFactory>().AsSingle();

            // States
            Container.Bind<PlayerIdleState>().AsTransient();
            Container.Bind<PlayerRunState>().AsTransient();
            Container.Bind<PlayerFinishingState>().AsTransient();
        }
    }
}