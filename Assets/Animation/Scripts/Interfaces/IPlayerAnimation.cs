using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления аниматором игрока.
    /// </summary>
    public interface IPlayerAnimation
    {
        Animator Animator { get; }
        void SetBool(string paramName, bool value);
        void SetFloat(string paramName, float value);
    }
}