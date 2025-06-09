using Animation.Scripts.Common;
using Animation.Scripts.Configs;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig gameConfig;

        public override void InstallBindings()
        {
            // --- CONFIGS ---
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
            Container.Bind<MovementConfig>().FromInstance(gameConfig.movementConfig).AsSingle();
            Container.Bind<FinisherConfig>().FromInstance(gameConfig.finisherConfig).AsSingle();
            Container.Bind<CameraConfig>().FromInstance(gameConfig.cameraConfig).AsSingle();
            Container.Bind<EnemyConfig>().FromInstance(gameConfig.enemyConfig).AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        }
    }
}