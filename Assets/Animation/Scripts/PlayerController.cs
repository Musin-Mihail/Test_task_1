using System;
using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за обработку пользовательского ввода и передачу команд.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public event Action OnMoveForwardPressed;
        public event Action OnMoveForwardReleased;
        public event Action OnMoveBackPressed;
        public event Action OnMoveBackReleased;
        public event Action OnMoveLeftPressed;
        public event Action OnMoveLeftReleased;
        public event Action OnMoveRightPressed;
        public event Action OnMoveRightReleased;
        public event Action OnSpacePressed;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.W))
            {
                OnMoveForwardPressed?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                OnMoveForwardReleased?.Invoke();
            }

            if (Input.GetKey(KeyCode.A))
            {
                OnMoveLeftPressed?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                OnMoveLeftReleased?.Invoke();
            }

            if (Input.GetKey(KeyCode.D))
            {
                OnMoveRightPressed?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                OnMoveRightReleased?.Invoke();
            }

            if (Input.GetKey(KeyCode.S))
            {
                OnMoveBackPressed?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                OnMoveBackReleased?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpacePressed?.Invoke();
            }
        }

        /// <summary>
        /// Возвращает текущую позицию мыши в мировых координатах.
        /// </summary>
        public Vector3 GetMouseWorldPosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, 1 << 8))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}