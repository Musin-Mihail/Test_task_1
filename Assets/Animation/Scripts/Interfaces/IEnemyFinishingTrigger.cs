namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для триггера добивания врага.
    /// </summary>
    public interface IEnemyFinishingTrigger
    {
        /// <summary>
        /// Проверка запуска добивания
        /// </summary>
        bool TryStartFinishing();
    }
}