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
        [SerializeField] private PlayerAnimationController animationController;

        public IPlayerAnimation PlayerAnimationInstance => playerAnimation;
        public IPlayerMovement PlayerMovementInstance => playerMovement;
        public IPlayerFinisher PlayerFinisherInstance => playerFinisher;
        public IPlayerController PlayerControllerInstance => playerController;
        public IEnemyFinishingTrigger EnemyFinishingTriggerInstance => enemyFinishingTrigger;
        private IPlayerEquipment PlayerEquipmentInstance => playerEquipment;
        private IEnemyFinisherHandler EnemyFinisherHandlerInstance => enemyFinisherHandler;
        private IPlayerAnimationController PlayerAnimationControllerInstance => animationController;

        private Collider _playerCollider;
        private PlayerMovementState _playerMovementState;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
            if (!_playerCollider)
            {
                Debug.LogError("Collider не найден на GameObject PlayerComponentRegistry.");
            }

            if (PlayerControllerInstance != null)
            {
                _playerMovementState = new PlayerMovementState(PlayerControllerInstance);
            }
            else
            {
                Debug.LogError("PlayerController не назначен. Невозможно инициализировать PlayerMovementState.");
            }

            if (PlayerAnimationControllerInstance != null)
            {
                PlayerAnimationControllerInstance.Initialize(PlayerMovementInstance, PlayerAnimationInstance);
            }
            else
            {
                Debug.LogError("PlayerAnimationControllerInstance не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (PlayerMovementInstance != null && _playerMovementState != null)
            {
                PlayerMovementInstance.Initialize(PlayerControllerInstance, PlayerFinisherInstance, PlayerAnimationControllerInstance, _playerMovementState);
            }
            else
            {
                Debug.LogError("PlayerMovement не назначен или PlayerMovementState не инициализирован в инспекторе PlayerComponentRegistry.");
            }

            if (PlayerFinisherInstance != null)
            {
                if (_playerCollider)
                {
                    PlayerFinisherInstance.Initialize(_playerCollider, PlayerAnimationInstance, PlayerMovementInstance, PlayerEquipmentInstance);
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
                EnemyFinishingTriggerInstance.Initialize(PlayerFinisherInstance);
            }
            else
            {
                Debug.LogError("EnemyFinishingTrigger не назначен в инспекторе PlayerComponentRegistry.");
            }

            if (EnemyFinisherHandlerInstance != null)
            {
                EnemyFinisherHandlerInstance.Initialize(PlayerFinisherInstance);
            }
            else
            {
                Debug.LogError("EnemyFinisherHandler не назначен в инспекторе PlayerComponentRegistry.");
            }
        }

        private void OnDestroy()
        {
            _playerMovementState?.Dispose();
        }
    }
}