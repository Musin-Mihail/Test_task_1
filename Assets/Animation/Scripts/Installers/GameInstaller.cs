using Animation.Scripts.Managers;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(FindFirstObjectByType<GameManager>()).AsSingle();
        }
    }
}