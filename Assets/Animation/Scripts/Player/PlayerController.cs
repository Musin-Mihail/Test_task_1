using System;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за обработку пользовательского ввода и передачу команд.
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public LayerMask mouseWorldPositionLayerMask;
        public event Action<MovementDirection, KeyState> OnMovementIntent;
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
                OnMovementIntent?.Invoke(MovementDirection.Forward, KeyState.Pressed);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                OnMovementIntent?.Invoke(MovementDirection.Forward, KeyState.Released);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                OnMovementIntent?.Invoke(MovementDirection.Forward, KeyState.Down);
            }

            if (Input.GetKey(KeyCode.A))
            {
                OnMovementIntent?.Invoke(MovementDirection.Left, KeyState.Pressed);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                OnMovementIntent?.Invoke(MovementDirection.Left, KeyState.Released);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnMovementIntent?.Invoke(MovementDirection.Left, KeyState.Down);
            }

            if (Input.GetKey(KeyCode.D))
            {
                OnMovementIntent?.Invoke(MovementDirection.Right, KeyState.Pressed);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                OnMovementIntent?.Invoke(MovementDirection.Right, KeyState.Released);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnMovementIntent?.Invoke(MovementDirection.Right, KeyState.Down);
            }

            if (Input.GetKey(KeyCode.S))
            {
                OnMovementIntent?.Invoke(MovementDirection.Back, KeyState.Pressed);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                OnMovementIntent?.Invoke(MovementDirection.Back, KeyState.Released);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                OnMovementIntent?.Invoke(MovementDirection.Back, KeyState.Down);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpacePressed?.Invoke();
            }
        }

        public Vector3 GetMouseWorldPosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, mouseWorldPositionLayerMask))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}