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
        /// Определяет доминирующее направление движения игрока и воспроизводит соответствующую анимацию.
        /// </summary>
        void UpdateAndPlayMovementAnimation();
    }
}