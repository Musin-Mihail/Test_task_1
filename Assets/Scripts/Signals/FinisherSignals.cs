using UnityEngine;

namespace Animation.Scripts.Signals
{
    /// <summary>
    /// Сигнал, отправляемый из анимации, когда достигается точка удара
    /// </summary>
    public class FinisherImpactPointReachedSignal
    {
    }

    /// <summary>
    /// Сигнал, отправляемый из анимации, когда она полностью завершена
    /// </summary>
    public class FinisherAnimationCompleteSignal
    {
    }

    /// <summary>
    /// Сигнал, который отправляет враг, когда его можно добить
    /// </summary>
    public struct EnemyReadyForFinisherSignal
    {
        public Transform EnemyTransform;
    }

    public struct EnemyExitedFinisherRangeSignal
    {
    }
}