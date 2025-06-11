using System.Collections;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public interface ITargetMover
    {
        IEnumerator MoveToTarget(Transform transformToMove, Vector3 targetPosition, float stopDistance, float speed);
    }

    public class TargetMover : ITargetMover
    {
        public IEnumerator MoveToTarget(Transform transformToMove, Vector3 targetPosition, float stopDistance, float speed)
        {
            while (Vector3.Distance(transformToMove.position, targetPosition) > stopDistance)
            {
                var direction = (targetPosition - transformToMove.position).normalized;
                transformToMove.position += direction * (speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}