using System;
using Animation.Scripts.Constants;
using Animation.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за обработку пользовательского ввода и передачу команд с использованием новой системы ввода Unity.
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public LayerMask mouseWorldPositionLayerMask;
        public event Action<MovementDirection, KeyState> OnMovementIntent;
        public event Action OnSpacePressed;

        private Camera _camera;
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Player.MoveForward.performed += ctx => OnMovementIntent?.Invoke(MovementDirection.Forward, KeyState.Pressed);
            _playerInputActions.Player.MoveForward.canceled += ctx => OnMovementIntent?.Invoke(MovementDirection.Forward, KeyState.Released);

            _playerInputActions.Player.MoveBack.performed += ctx => OnMovementIntent?.Invoke(MovementDirection.Back, KeyState.Pressed);
            _playerInputActions.Player.MoveBack.canceled += ctx => OnMovementIntent?.Invoke(MovementDirection.Back, KeyState.Released);

            _playerInputActions.Player.MoveLeft.performed += ctx => OnMovementIntent?.Invoke(MovementDirection.Left, KeyState.Pressed);
            _playerInputActions.Player.MoveLeft.canceled += ctx => OnMovementIntent?.Invoke(MovementDirection.Left, KeyState.Released);

            _playerInputActions.Player.MoveRight.performed += ctx => OnMovementIntent?.Invoke(MovementDirection.Right, KeyState.Pressed);
            _playerInputActions.Player.MoveRight.canceled += ctx => OnMovementIntent?.Invoke(MovementDirection.Right, KeyState.Released);

            _playerInputActions.Player.Finishing.performed += ctx => OnSpacePressed?.Invoke();

            _playerInputActions.Player.Quit.performed += ctx => Application.Quit();
        }

        private void OnEnable()
        {
            _playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Player.Disable();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        /// <summary>
        /// Возвращает мировую позицию, на которую указывает мышь.
        /// </summary>
        public Vector3 GetMouseWorldPosition()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, mouseWorldPositionLayerMask))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}