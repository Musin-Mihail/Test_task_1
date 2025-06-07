namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Определяет контракт для контроллера анимации игрока.
    /// </summary>
    public interface IPlayerAnimationController
    {
        /// <summary>
        /// Обновляет параметры аниматора в зависимости от движения игрока.
        /// </summary>
        void UpdateAndPlayMovementAnimation();
    }
}