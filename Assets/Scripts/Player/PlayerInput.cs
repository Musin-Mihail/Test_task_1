using Animation.Scripts.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public interface IPlayerInput
{
    Vector3 GetMouseWorldPosition();
}

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] private LayerMask mouseWorldPositionLayerMask;

    private SignalBus _signalBus;
    private Camera _camera;
    private PlayerInputActions _playerInputActions;

    [Inject]
    public void Construct(SignalBus signalBus, Camera camera)
    {
        _signalBus = signalBus;
        _camera = camera;
    }

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Move.performed += ctx => _signalBus.Fire(new MovementInputSignal { InputValue = ctx.ReadValue<Vector2>() });
        _playerInputActions.Player.Move.canceled += ctx => _signalBus.Fire(new MovementInputSignal { InputValue = Vector2.zero });
        _playerInputActions.Player.Finishing.performed += ctx => _signalBus.Fire<FinisherButtonSignal>();
        _playerInputActions.Player.Quit.performed += ctx => _signalBus.Fire<QuitGameSignal>();
    }

    private void OnEnable() => _playerInputActions.Enable();
    private void OnDisable() => _playerInputActions.Disable();

    public Vector3 GetMouseWorldPosition()
    {
        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out var hit, float.PositiveInfinity, mouseWorldPositionLayerMask)
            ? hit.point
            : Vector3.zero;
    }
}