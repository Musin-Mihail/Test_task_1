using System.Collections;
using Animation.Scripts.Common;
using Animation.Scripts.Configs;
using Animation.Scripts.Constants;
using Animation.Scripts.Player;
using UnityEngine;

namespace Animation.Scripts.FSM
{
    public class PlayerApproachingState : PlayerState
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITargetMover _targetMover;
        private readonly IPlayerAnimation _animation;
        private readonly IPlayerRotator _rotator;
        private readonly FinisherConfig _config;
        private readonly Transform _playerTransform;
        private readonly FinisherAvailabilityService _availabilityService;

        private Coroutine _approachCoroutine;

        public PlayerApproachingState(
            ICoroutineRunner coroutineRunner,
            ITargetMover targetMover,
            IPlayerAnimation animation,
            IPlayerRotator rotator,
            FinisherConfig config,
            Transform playerTransform,
            FinisherAvailabilityService availabilityService)
        {
            _coroutineRunner = coroutineRunner;
            _targetMover = targetMover;
            _animation = animation;
            _rotator = rotator;
            _config = config;
            _playerTransform = playerTransform;
            _availabilityService = availabilityService;
        }

        public override void Enter()
        {
            if (!_availabilityService.IsFinisherAvailable)
            {
                StateMachine.IsFinisherRequested = false;
                return;
            }

            StateMachine.IsTargetReached = false;
            _availabilityService.SetFinisherInProgress(true);
            _approachCoroutine = _coroutineRunner.StartCoroutine(ApproachCoroutine());
        }

        private IEnumerator ApproachCoroutine()
        {
            _rotator.RotateToTarget(_availabilityService.FinisherTarget.position);
            _animation.SetBool(AnimationConstants.IsMoving, true);

            yield return _targetMover.MoveToTarget(
                _playerTransform,
                _availabilityService.FinisherTarget.position,
                _config.finishingStartDistance,
                _config.finishingMovementSpeed
            );

            _animation.SetBool(AnimationConstants.IsMoving, false);
            StateMachine.IsTargetReached = true;
        }

        public override void Exit()
        {
            if (_approachCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_approachCoroutine);
                _approachCoroutine = null;
            }

            _animation.SetBool(AnimationConstants.IsMoving, false);
        }
    }
}