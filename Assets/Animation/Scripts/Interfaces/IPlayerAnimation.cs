using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления аниматором игрока.
    /// </summary>
    public interface IPlayerAnimation
    {
        Animator Animator { get; }
        /// <summary>
        /// Устанавливает параметр булевого типа в аниматоре.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="value">Значение параметра.</param>
        void SetBool(string paramName, bool value);
        /// <summary>
        /// Устанавливает параметр float типа в аниматоре.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="value">Значение параметра.</param>
        void SetFloat(string paramName, float value);
    }
}