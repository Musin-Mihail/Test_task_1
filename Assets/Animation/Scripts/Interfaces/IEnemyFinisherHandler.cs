namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для обработчика добивания противника.
    /// </summary>
    public interface IEnemyFinisherHandler
    {
        /// <summary>
        /// Инициализирует обработчик добивания противника и подписывается на событие PlayerFinisher.
        /// </summary>
        /// <param name="playerFinisher">Ссылка на IPlayerFinisher.</param>
        void Initialize(IPlayerFinisher playerFinisher);
    }
}