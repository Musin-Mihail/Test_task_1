namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления вращением игрока.
    /// </summary>
    public interface IPlayerRotator
    {
        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerController">Ссылка на IPlayerController.</param>
        /// <param name="playerFinisher">Ссылка на IPlayerFinisher.</param>
        void Initialize(IPlayerController playerController, IPlayerFinisher playerFinisher);

        /// <summary>
        /// Поворачивает объект игрока в сторону указателя мыши.
        /// </summary>
        void RotationToMouse();

        /// <summary>
        /// Поворачивает объект игрока в сторону цели.
        /// </summary>
        void RotationToTarget();

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
        void RotateTowardsCamera();
    }
}