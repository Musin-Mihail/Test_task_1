using Animation.Scripts.Enemy;
using UnityEngine;

namespace Animation.Scripts.Player
{
    public class PlayerComponentRegistry : MonoBehaviour
    {
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerFinisher playerFinisher;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private EnemyFinishingTrigger enemyFinishingTrigger;
        [SerializeField] private PlayerEquipment playerEquipment;
        [SerializeField] private EnemyFinisherHandler enemyFinisherHandler;

        public PlayerAnimation PlayerAnimation => playerAnimation;
        public PlayerMovement PlayerMovement => playerMovement;
        public PlayerFinisher PlayerFinisher => playerFinisher;
        public PlayerController PlayerController => playerController;
        public EnemyFinishingTrigger EnemyFinishingTrigger => enemyFinishingTrigger;

        private Collider _playerCollider;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
            if (!_playerCollider)
            {
                Debug.LogError("Collider не найден на GameObject PlayerComponentRegistry.");
            }

            if (playerMovement)
            {
                playerMovement.Initialize(playerController, playerFinisher);
            }
            else
            {
                Debug.LogError("PlayerMovement не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (playerFinisher)
            {
                if (_playerCollider)
                {
                    playerFinisher.Initialize(_playerCollider, playerAnimation, playerMovement, playerEquipment);
                }
                else
                {
                    Debug.LogError("PlayerFinisher не может быть инициализирован: Collider не найден.");
                }
            }
            else
            {
                Debug.LogError("PlayerFinisher не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (enemyFinishingTrigger)
            {
                enemyFinishingTrigger.Initialize(playerFinisher);
            }
            else
            {
                Debug.LogError("EnemyFinishingTrigger не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (enemyFinisherHandler)
            {
                enemyFinisherHandler.Initialize(playerFinisher);
            }
            else
            {
                Debug.LogError("EnemyFinisherHandler не назначен в инспекторе PlayerComponentRegistry.");
            }
        }
    }
}