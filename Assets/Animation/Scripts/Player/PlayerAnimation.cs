using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за управление аниматором игрока.
    /// Реализует интерфейс IPlayerAnimation.
    /// </summary>
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        public Animator animator;

        /// <summary>
        /// Проигрывает указанную анимацию.
        /// </summary>
        /// <param name="animationName">Название анимации для проигрывания.</param>
        public void PlayAnimation(string animationName)
        {
            if (animator)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                {
                    animator.Play(animationName);
                }
            }
        }
    }
}