using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    public interface IPlayerMovement
    {
        bool IsMovingForward { get; }
        bool IsMovingBack { get; }
        bool IsMovingLeft { get; }
        bool IsMovingRight { get; }
        void Initialize(IPlayerController controller, IPlayerFinisher finisher, IPlayerAnimationController animationController);
        void Move();
        void RotationToMouse();
        void RotationToTarget();
        void RotateTowardsCamera();
        IEnumerator MoveToTarget(Vector3 targetPosition, float stopDistance);
        bool IsMoving();
        void UpdateMovementAndAnimation();
    }
}