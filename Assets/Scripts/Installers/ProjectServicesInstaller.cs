using Animation.Scripts.Common;
using Animation.Scripts.Player;
using UnityEngine;
using Zenject;
using ITargetMover = Animation.Scripts.Player.ITargetMover;

namespace Animation.Scripts.Installers
{
    public class ProjectServicesInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner coroutineRunner;

        public override void InstallBindings()
        {
            // Coroutine Runner
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromInstance(coroutineRunner).AsSingle();

            // Target Mover
            Container.Bind<ITargetMover>().To<TargetMover>().AsSingle();
        }
    }
}