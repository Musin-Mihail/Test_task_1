using Animation.Scripts.Enemy;
using Animation.Scripts.Interfaces;
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

        public IPlayerAnimation PlayerAnimationInstance => playerAnimation;
        public IPlayerMovement PlayerMovementInstance => playerMovement;
        public IPlayerFinisher PlayerFinisherInstance => playerFinisher;
        public IPlayerController PlayerControllerInstance => playerController;
        public IEnemyFinishingTrigger EnemyFinishingTriggerInstance => enemyFinishingTrigger;
        private IPlayerEquipment PlayerEquipmentInstance => playerEquipment;
        private IEnemyFinisherHandler EnemyFinisherHandlerInstance => enemyFinisherHandler;

        private Collider _playerCollider;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
            if (!_playerCollider)
            {
                Debug.LogError("Collider не найден на GameObject PlayerComponentRegistry.");
            }

            if (PlayerMovementInstance != null)
            {
                PlayerMovementInstance.Initialize(playerController, playerFinisher);
            }
            else
            {
                Debug.LogError("PlayerMovement не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (PlayerFinisherInstance != null)
            {
                if (_playerCollider)
                {
                    PlayerFinisherInstance.Initialize(_playerCollider, playerAnimation, PlayerMovementInstance, PlayerEquipmentInstance);
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

            if (EnemyFinishingTriggerInstance != null)
            {
                EnemyFinishingTriggerInstance.Initialize(playerFinisher);
            }
            else
            {
                Debug.LogError("EnemyFinishingTrigger не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (EnemyFinisherHandlerInstance != null)
            {
                EnemyFinisherHandlerInstance.Initialize(playerFinisher);
            }
            else
            {
                Debug.LogError("EnemyFinisherHandler не назначен в инспекторе PlayerComponentRegistry.");
            }
        }
    }
}