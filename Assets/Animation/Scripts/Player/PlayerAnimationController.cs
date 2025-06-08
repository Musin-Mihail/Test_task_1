using Animation.Scripts.Interfaces;
using Animation.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class PlayerAnimationController : IPlayerAnimationController
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveZ = Animator.StringToHash("MoveZ");

        private readonly MovementConfig _movementConfig;
        private readonly IPlayerMovement _playerMovement;
        private readonly IPlayerAnimation _playerAnimation;

        [Inject]
        public PlayerAnimationController(IPlayerMovement playerMovement, IPlayerAnimation playerAnimation, MovementConfig movementConfig)
        {
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;
            _movementConfig = movementConfig;
        }

        public void UpdateAndPlayMovementAnimation()
        {
            if (_playerMovement == null || _playerAnimation == null || !_playerAnimation.Animator || !_movementConfig)
            {
                return;
            }

            var targetMoveDirection = _playerMovement.CurrentMovementInput;

            var targetMoveX = targetMoveDirection.x;
            var targetMoveZ = targetMoveDirection.z;

            var currentMoveX = _playerAnimation.Animator.GetFloat(MoveX);
            var currentMoveZ = _playerAnimation.Animator.GetFloat(MoveZ);

            currentMoveX = Mathf.Lerp(currentMoveX, targetMoveX, Time.deltaTime / _movementConfig.animationSmoothTime);
            currentMoveZ = Mathf.Lerp(currentMoveZ, targetMoveZ, Time.deltaTime / _movementConfig.animationSmoothTime);

            _playerAnimation.SetFloat("MoveX", currentMoveX);
            _playerAnimation.SetFloat("MoveZ", currentMoveZ);
            _playerAnimation.SetBool("IsMoving", _playerMovement.IsMoving());
        }
    }
}