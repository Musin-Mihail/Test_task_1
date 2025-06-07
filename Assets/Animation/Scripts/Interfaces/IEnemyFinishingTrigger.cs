namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для триггера добивания врага.
    /// </summary>
    public interface IEnemyFinishingTrigger
    {
        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerFinisher">Ссылка на IPlayerFinisher.</param>
        void Initialize(IPlayerFinisher playerFinisher);

        /// <summary>
        /// Проверка запуска добивания
        /// </summary>
        bool TryStartFinishing();
    }
}