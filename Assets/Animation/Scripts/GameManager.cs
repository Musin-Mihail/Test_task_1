using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за общие настройки игры.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Целевая частота кадров для приложения.")]
        public int targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}