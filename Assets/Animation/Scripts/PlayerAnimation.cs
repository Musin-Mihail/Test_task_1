using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за управление аниматором игрока.
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator animator;
        public Transform chest;

        private PlayerController _playerController;
        private PlayerMovement _playerMovement;
        private PlayerCombat _playerCombat;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerCombat = GetComponent<PlayerCombat>();
        }

        private void LateUpdate()
        {
            UpdateAnimations(_playerController.GetMouseWorldPosition());
        }

        /// <summary>
        /// Обновляет анимации игрока на основе его состояния.
        /// </summary>
        /// <param name="mouseWorldPosition">Позиция мыши в мировых координатах для поворота груди.</param>
        private void UpdateAnimations(Vector3 mouseWorldPosition)
        {
            if (_playerCombat.IsFinishing())
            {
                chest.rotation = Quaternion.LookRotation(Vector3.up, _playerCombat.CurrentTarget - chest.position);
                chest.transform.Rotate(-30, 90, 10); // Применяем фиксированные повороты для добивания
                transform.rotation = Quaternion.LookRotation(Vector3.up, _playerCombat.CurrentTarget - transform.position);
                transform.transform.Rotate(270, 180, 0);
            }
            else if (_playerMovement.IsMoving())
            {
                chest.rotation = Quaternion.LookRotation(Vector3.up, mouseWorldPosition - chest.position);
                chest.transform.Rotate(-30, 90, 0);

                if (_playerMovement.IsMovingForward)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Rifle"))
                    {
                        animator.Play("Run_Rifle");
                    }
                }
                else if (_playerMovement.IsMovingBack)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Back_Run_Rifle"))
                    {
                        animator.Play("Back_Run_Rifle");
                    }
                }
                else if (_playerMovement.IsMovingLeft)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Left_Rifle"))
                    {
                        animator.Play("Run_Left_Rifle");
                    }
                }
                else if (_playerMovement.IsMovingRight)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Right_Rifle"))
                    {
                        animator.Play("Run_Right_Rifle");
                    }
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.Play("Idle");
                }
            }
        }

        /// <summary>
        /// Проигрывает указанную анимацию.
        /// </summary>
        /// <param name="animationName">Название анимации для проигрывания.</param>
        public void PlayAnimation(string animationName)
        {
            if (animator)
            {
                animator.Play(animationName);
            }
        }
    }
}