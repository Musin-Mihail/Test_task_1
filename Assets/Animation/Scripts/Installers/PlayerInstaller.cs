using Animation.Scripts.Enemy;
using Animation.Scripts.Interfaces;
using Animation.Scripts.Player;
using Animation.Scripts.ScriptableObjects;
using Animation.Scripts.Signals;
using Animation.Scripts.States;
using UnityEngine;
using Zenject;

// Добавили using для сигналов

namespace Animation.Scripts.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerEquipment playerEquipment;
        [SerializeField] private EnemyFinishingTrigger enemyFinishingTrigger;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private Transform chestTransform;
        [SerializeField] private GameObject enemyGameObject;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private PlayerStateMachine playerStateMachine;
        [SerializeField] private AnimationEventBridge animationEventBridge;
        [SerializeField] private CameraController cameraController; // Добавили ссылку на CameraController

        public override void InstallBindings()
        {
            // --- СИГНАЛЫ ---
            // Объявляем сигналы, чтобы Zenject мог их обрабатывать
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<FinisherImpactSignal>();
            Container.DeclareSignal<FinisherAnimationCompleteSignal>();

            // --- КОНФИГУРАЦИИ (ScriptableObjects) ---
            // Биндим главный конфиг и его составные части
            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();
            Container.Bind<MovementConfig>().FromInstance(playerConfig.movementConfig).AsSingle();
            Container.Bind<FinisherConfig>().FromInstance(playerConfig.finisherConfig).AsSingle();
            Container.Bind<CameraConfig>().FromInstance(playerConfig.cameraConfig).AsSingle();

            // --- ОБЪЕКТЫ НА СЦЕНЕ (MonoBehaviour) ---
            Container.BindInterfacesAndSelfTo<PlayerAnimation>().FromInstance(playerAnimation).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(playerController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerEquipment>().FromInstance(playerEquipment).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinishingTrigger>().FromInstance(enemyFinishingTrigger).AsSingle();
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromInstance(coroutineRunner).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().FromInstance(playerStateMachine).AsSingle();
            Container.Bind<AnimationEventBridge>().FromInstance(animationEventBridge).AsSingle();
            Container.Bind<CameraController>().FromInstance(cameraController).AsSingle();

            // --- ОБЫЧНЫЕ КЛАССЫ (POCO) ---
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFinisher>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAnimationController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinisherHandler>().AsSingle().NonLazy();
            Container.Bind<ITargetMover>().To<TargetMover>().AsSingle();
            Container.Bind<EnemyLifecycleManager>().AsSingle();

            // --- МАШИНА СОСТОЯНИЙ ---
            Container.Bind<StateFactory>().AsSingle();
            Container.Bind<PlayerMovementState>().AsSingle();
            Container.Bind<PlayerIdleState>().AsTransient();
            Container.Bind<PlayerRunState>().AsTransient();
            Container.Bind<PlayerFinishingState>().AsTransient();

            // --- КОМПОНЕНТЫ И ТРАНСФОРМЫ ---
            Container.Bind<Transform>().WithId("PlayerChestTransform").FromInstance(chestTransform).AsCached();
            Container.Bind<Transform>().WithId("PlayerTransform").FromInstance(playerTransform).AsCached();
            Container.Bind<GameObject>().WithId("EnemyGameObject").FromInstance(enemyGameObject).AsSingle();
            Container.Bind<Collider>().FromInstance(GetComponent<Collider>()).AsSingle();
        }
    }
}