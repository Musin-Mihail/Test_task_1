using Animation.Scripts.Enemy;
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
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerFinisher playerFinisher;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private EnemyFinishingTrigger enemyFinishingTrigger;
        [SerializeField] private PlayerEquipment playerEquipment;
        [SerializeField] private EnemyFinisherHandler enemyFinisherHandler;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        [SerializeField] private PlayerRotator playerRotator;
        [SerializeField] private PlayerConfig playerConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerAnimation>().FromInstance(playerAnimation).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovement>().FromInstance(playerMovement).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFinisher>().FromInstance(playerFinisher).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().FromInstance(playerController).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinishingTrigger>().FromInstance(enemyFinishingTrigger).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerEquipment>().FromInstance(playerEquipment).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAnimationController>().FromInstance(playerAnimationController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRotator>().FromInstance(playerRotator).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().FromInstance(GetComponent<PlayerStateMachine>()).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFinisherHandler>().FromInstance(enemyFinisherHandler).AsSingle();

            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();
            Container.Bind<PlayerMovementState>().AsSingle();

            Container.Bind<Collider>().FromInstance(GetComponent<Collider>()).AsSingle();
        }
    }
}