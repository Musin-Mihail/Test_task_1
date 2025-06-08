using Animation.Scripts.Enemy;
using Animation.Scripts.Interfaces;
using Animation.Scripts.Player;
using Animation.Scripts.ScriptableObjects;
using Animation.Scripts.States;
using UnityEngine;
using Zenject;

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

        public override void InstallBindings()
        {
            // Привязки для Transform
            Container.Bind<Transform>().WithId("PlayerChestTransform").FromInstance(chestTransform).AsCached();
            Container.Bind<Transform>().WithId("PlayerTransform").FromInstance(playerTransform).AsCached();
            Container.Bind<GameObject>().WithId("EnemyGameObject").FromInstance(enemyGameObject).AsSingle();

            // Привязки для MonoBehaviour классов, которые должны быть на сцене
            Container.BindInterfacesAndSelfTo<PlayerAnimation>().FromInstance(playerAnimation).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(playerController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerEquipment>().FromInstance(playerEquipment).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinishingTrigger>().FromInstance(enemyFinishingTrigger).AsSingle();
            Container.BindInterfacesAndSelfTo<ICoroutineRunner>().FromInstance(coroutineRunner).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().FromInstance(playerStateMachine).AsSingle();

            // Привязки для POCO классов - Zenject будет создавать их сам
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFinisher>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAnimationController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinisherHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TargetMover>().AsSingle();

            // PlayerConfig остаётся FromInstance, так как это ScriptableObject
            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();

            // Привязка состояний игрока Zenject'ом
            Container.Bind<PlayerMovementState>().AsSingle();
            Container.Bind<PlayerIdleState>().AsTransient();
            Container.Bind<PlayerRunState>().AsTransient();
            Container.Bind<PlayerFinishingState>().AsTransient();

            // Collider также остаётся FromInstance, так как он прикреплен к GameObject
            Container.Bind<Collider>().FromInstance(GetComponent<Collider>()).AsSingle();
        }
    }
}