using Animation.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за триггер добивания врага.
    /// </summary>
    public class EnemyFinishingTrigger : MonoBehaviour, IEnemyFinishingTrigger
    {
        public GameObject finishingText;
        private IPlayerFinisher _playerFinisher;
        private Vector3 _currentTarget;

        [Inject]
        public void Construct(IPlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _currentTarget = other.gameObject.transform.position;
                _currentTarget.y = 0;
                finishingText.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                finishingText.SetActive(false);
            }
        }

        public bool TryStartFinishing()
        {
            if (finishingText.activeSelf && !_playerFinisher.IsFinishing())
            {
                _playerFinisher.TargetPosition = _currentTarget;
                finishingText.SetActive(false);
                return true;
            }

            return false;
        }
    }
}