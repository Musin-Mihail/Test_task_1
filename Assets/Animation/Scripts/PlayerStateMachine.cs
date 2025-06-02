using Animation.Scripts.States;
using UnityEngine;

namespace Animation.Scripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerState CurrentState { get; set; }
        public PlayerAnimation PlayerAnimation { get; private set; }
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerCombat PlayerCombat { get; private set; }
        public PlayerController PlayerController { get; private set; }

        private void Awake()
        {
            PlayerAnimation = GetComponent<PlayerAnimation>();
            PlayerMovement = GetComponent<PlayerMovement>();
            PlayerCombat = GetComponent<PlayerCombat>();
            PlayerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            PlayerMovement.RotateTowardsCamera();
            ChangeState(new PlayerIdleState(this));
        }

        private void Update()
        {
            CurrentState?.UpdateState();
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdateState();
        }

        private void LateUpdate()
        {
            CurrentState?.LateUpdateState();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}