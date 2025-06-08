using System;
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
        public event Action<Vector2> OnMovementInput;
        public event Action OnSpacePressed;

        private Camera _camera;
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Player.Move.performed += ctx => OnMovementInput?.Invoke(ctx.ReadValue<Vector2>());
            _playerInputActions.Player.Move.canceled += ctx => OnMovementInput?.Invoke(Vector2.zero);

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