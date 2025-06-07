namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Определяет контракт для контроллера анимации игрока.
    /// </summary>
    public interface IPlayerAnimationController
    {
        /// <summary>
        /// Инициализирует контроллер анимации.
        /// </summary>
        /// <param name="playerMovement">Ссылка на IPlayerMovement для получения данных о движении.</param>
        /// <param name="playerAnimation">Ссылка на IPlayerAnimation для воспроизведения анимаций.</param>
        void Initialize(IPlayerMovement playerMovement, IPlayerAnimation playerAnimation);

        /// <summary>
        /// Обновляет параметры аниматора в зависимости от движения игрока.
        /// </summary>
        void UpdateAndPlayMovementAnimation();
    }
}