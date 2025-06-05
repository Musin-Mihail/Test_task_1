namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для триггера добивания врага.
    /// </summary>
    public interface IEnemyFinishingTrigger
    {
        void Initialize(IPlayerFinisher playerFinisher);
        bool TryStartFinishing();
    }
}