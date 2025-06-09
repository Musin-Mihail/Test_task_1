using Animation.Scripts.Signals;
using Zenject;

namespace Animation.Scripts.Installers
{
    public class GameSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            // Input
            Container.DeclareSignal<MovementInputSignal>();
            Container.DeclareSignal<FinisherButtonSignal>();
            Container.DeclareSignal<QuitGameSignal>();

            // Finisher
            Container.DeclareSignal<FinisherImpactPointReachedSignal>();
            Container.DeclareSignal<FinisherAnimationCompleteSignal>();
            Container.DeclareSignal<EnemyReadyForFinisherSignal>();
            Container.DeclareSignal<EnemyExitedFinisherRangeSignal>();

            // Game State
            Container.DeclareSignal<GameStateSignals.RequestStateChangeSignal>();
        }
    }
}