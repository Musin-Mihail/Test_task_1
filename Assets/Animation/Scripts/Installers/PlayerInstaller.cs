using Animation.Scripts.Configs;
using Animation.Scripts.FSM;
using Animation.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerFacade playerFacade;
        [SerializeField] private PlayerConfig playerConfig;

        public override void InstallBindings()
        {
            // --- CONFIGS ---
            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();
            Container.Bind<MovementConfig>().FromInstance(playerConfig.movementConfig).AsSingle();
            Container.Bind<FinisherConfig>().FromInstance(playerConfig.finisherConfig).AsSingle();
            Container.Bind<CameraConfig>().FromInstance(playerConfig.cameraConfig).AsSingle();

            // --- FACADE & COMPONENTS ---
            Container.Bind<PlayerFacade>().FromInstance(playerFacade).AsSingle();
            Container.Bind<Transform>().FromInstance(playerFacade.transform).AsCached();
            Container.Bind<Animator>().FromInstance(playerFacade.Animator).AsSingle();
            Container.Bind<Transform>().WithId("ChestTransform").FromInstance(playerFacade.ChestTransform).AsCached();

            // --- MONOBEHAVIOURS ---
            Container.Bind<IPlayerInput>().To<PlayerInput>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerEquipment>().To<PlayerEquipment>().FromInstance(playerFacade.PlayerEquipment).AsSingle();
            Container.Bind<ICameraController>().To<CameraController>().FromComponentInHierarchy().AsSingle();

            // --- POCO (Plain Old C# Object) SERVICES ---
            Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFinisher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AnimationEventBridge>().AsSingle().NonLazy();

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