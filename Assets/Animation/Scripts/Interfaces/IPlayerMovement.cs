using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Interfaces
{
    /// <summary>
    /// Интерфейс для управления логикой перемещения игрока.
    /// </summary>
    public interface IPlayerMovement
    {
        void Initialize(IPlayerController controller, IPlayerFinisher finisher);
        void Move();
        void RotationToMouse();
        void RotationToTarget();
        void RotateTowardsCamera();
        IEnumerator MoveToTarget(Vector3 targetPosition, float stopDistance);
        bool IsMoving();
    }
}