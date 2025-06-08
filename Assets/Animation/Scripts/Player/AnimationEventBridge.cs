using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Компонент MonoBehaviour, который принимает вызовы событий анимации
    /// и перенаправляет их зарегистрированному обработчику IAnimationEventHandler.
    /// Этот компонент должен быть прикреплен к тому же GameObject, что и Animator.
    /// </summary>
    public class AnimationEventBridge : MonoBehaviour
    {
        private IAnimationEventHandler _animationEventHandler;

        /// <summary>
        /// Метод Construct для инъекции зависимостей с использованием Zenject.
        /// </summary>
        /// <param name="animationEventHandler">Обработчик событий анимации.</param>
        [Inject]
        public void Construct(IAnimationEventHandler animationEventHandler)
        {
            _animationEventHandler = animationEventHandler;
        }

        /// <summary>
        /// Этот метод будет вызываться из Animation Event в редакторе Unity.
        /// Он передает имя события зарегистрированному обработчику.
        /// </summary>
        /// <param name="eventName">Имя события (строка), определенное в Animation Event.</param>
        public void TriggerEvent(string eventName)
        {
            if (_animationEventHandler != null)
            {
                _animationEventHandler.HandleAnimationEvent(eventName);
            }
            else
            {
                Debug.LogError($"AnimationEventBridge: No IAnimationEventHandler assigned for event '{eventName}' on GameObject {gameObject.name}.");
            }
        }
    }
}