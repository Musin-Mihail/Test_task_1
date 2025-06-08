namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Универсальный интерфейс для обработки событий анимации.
    /// </summary>
    public interface IAnimationEventHandler
    {
        /// <summary>
        /// Обрабатывает событие анимации по имени.
        /// </summary>
        /// <param name="eventName">Имя события, вызванного из анимации.</param>
        void HandleAnimationEvent(string eventName);
    }
}