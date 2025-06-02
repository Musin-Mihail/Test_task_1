using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за управление аниматором игрока.
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
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