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

        public void SetBool(string paramName, bool value)
        {
            if (Animator)
            {
                Animator.SetBool(paramName, value);
            }
        }

        public void SetFloat(string paramName, float value)
        {
            if (Animator)
            {
                Animator.SetFloat(paramName, value);
            }
        }
    }
}