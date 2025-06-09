using Animation.Scripts.Enemy;
using Animation.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private PlayerFacade playerFacade;

        public override void InstallBindings()
        {
            Container.Bind<PlayerFacade>().FromInstance(playerFacade).AsSingle();
            // --- COMPONENTS ---
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Transform>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameObject>().FromInstance(gameObject).AsSingle();

            // --- POCO ---
            Container.BindInterfacesAndSelfTo<EnemyFinisherHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyLifecycleManager>().AsSingle().NonLazy();
        }
    }
}