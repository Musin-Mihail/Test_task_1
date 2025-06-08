using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    public class PlayerFinisherMonoBridge : MonoBehaviour
    {
        private IPlayerFinisher _playerFinisher;

        [Inject]
        public void Construct(IPlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
        }

        public void AnimationEvent_FinisherImpactPoint()
        {
            if (_playerFinisher != null)
            {
                _playerFinisher.AnimationEvent_FinisherImpactPoint();
            }
            else
            {
                Debug.LogError("PlayerFinisher не внедрен в PlayerFinisherMonoBridge!");
            }
        }

        public void AnimationEvent_FinisherComplete()
        {
            if (_playerFinisher != null)
            {
                _playerFinisher.AnimationEvent_FinisherComplete();
            }
            else
            {
                Debug.LogError("PlayerFinisher не внедрен в PlayerFinisherMonoBridge!");
            }
        }
    }
}