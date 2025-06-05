using Animation.Scripts.Interfaces;
using UnityEngine;

namespace Animation.Scripts.Player
{
    /// <summary>
    /// Отвечает за триггер добивания врага.
    /// Реализует интерфейс IEnemyFinishingTrigger.
    /// </summary>
    public class EnemyFinishingTrigger : MonoBehaviour, IEnemyFinishingTrigger
    {
        public GameObject finishingText;
        private IPlayerFinisher _playerFinisher;
        private Vector3 _currentTarget;

        /// <summary>
        /// Метод инициализации для внедрения зависимостей.
        /// </summary>
        /// <param name="playerFinisher">Ссылка на IPlayerFinisher.</param>
        public void Initialize(IPlayerFinisher playerFinisher)
        {
            _playerFinisher = playerFinisher;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _currentTarget = other.gameObject.transform.position;
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