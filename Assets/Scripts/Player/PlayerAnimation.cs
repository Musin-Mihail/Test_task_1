using Animation.Scripts.Configs;
using Animation.Scripts.Constants;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public interface IPlayerAnimation
    {
        void SetBool(string paramName, bool value);
        void UpdateMovementAnimation(Vector3 movementInput);
    }

    public class PlayerAnimation : IPlayerAnimation
    {
        private readonly Animator _animator;
        private readonly MovementConfig _movementConfig;

        private static readonly int MoveXHash = Animator.StringToHash(AnimationConstants.MoveX);
        private static readonly int MoveZHash = Animator.StringToHash(AnimationConstants.MoveZ);

        public PlayerAnimation(Animator animator, MovementConfig movementConfig)
        {
            _animator = animator;
            _movementConfig = movementConfig;
        }

        public void SetBool(string paramName, bool value) => _animator.SetBool(paramName, value);

        public void UpdateMovementAnimation(Vector3 movementInput)
        {
            var currentMoveX = _animator.GetFloat(MoveXHash);
            var currentMoveZ = _animator.GetFloat(MoveZHash);

            var smoothedX = Mathf.Lerp(currentMoveX, movementInput.x, Time.deltaTime / _movementConfig.animationSmoothTime);
            var smoothedZ = Mathf.Lerp(currentMoveZ, movementInput.z, Time.deltaTime / _movementConfig.animationSmoothTime);

            _animator.SetFloat(MoveXHash, smoothedX);
            _animator.SetFloat(MoveZHash, smoothedZ);
        }
    }
}