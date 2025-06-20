﻿using Animation.Scripts.Enemy;
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
            Container.Bind<Transform>().WithId("PlayerTransform").FromInstance(playerFacade.transform).AsCached();

            // --- COMPONENTS ---
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Transform>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameObject>().FromInstance(gameObject).AsSingle();

            // --- POCO & MONOBEHAVIOURS ---
            Container.BindInterfacesAndSelfTo<EnemyFinisherHandler>().AsSingle().NonLazy();
            Container.Bind<EnemyLifecycleManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}