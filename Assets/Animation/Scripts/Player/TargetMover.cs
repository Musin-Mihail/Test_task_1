using System.Collections;
using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Реализация ITargetMover для перемещения Transform к цели.
    /// </summary>
    public class TargetMover : ITargetMover
    {
        public IEnumerator MoveToTarget(Transform transformToMove, Vector3 targetPosition, float stopDistance, float speed)
        {
            var distanceToTarget = Vector3.Distance(transformToMove.position, targetPosition);
            while (distanceToTarget > stopDistance)
            {
                var directionToTarget = (targetPosition - transformToMove.position).normalized;
                transformToMove.position += directionToTarget * (speed * Time.deltaTime);
                distanceToTarget = Vector3.Distance(transformToMove.position, targetPosition);
                yield return null;
            }
        }
    }
}