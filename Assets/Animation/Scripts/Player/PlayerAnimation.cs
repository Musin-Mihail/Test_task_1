using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за управление аниматором игрока.
    /// Предоставляет методы для установки параметров аниматора.
    /// </summary>
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        [SerializeField] private Animator animator;

        public Animator Animator => animator;

        /// <summary>
        /// Устанавливает параметр булевого типа в аниматоре.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="value">Значение параметра.</param>
        public void SetBool(string paramName, bool value)
        {
            if (Animator)
            {
                Animator.SetBool(paramName, value);
            }
        }

        /// <summary>
        /// Устанавливает параметр float типа в аниматоре.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="value">Значение параметра.</param>
        public void SetFloat(string paramName, float value)
        {
            if (Animator)
            {
                Animator.SetFloat(paramName, value);
            }
        }
    }
}