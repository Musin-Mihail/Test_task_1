using UnityEngine;

namespace Animation.Scripts
{
    /// <summary>
    /// Отвечает за логику перемещения игрока.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        private const float Speed = 10f;
        public bool IsMovingForward { get; private set; }
        public bool IsMovingBack { get; private set; }
        public bool IsMovingLeft { get; private set; }
        public bool IsMovingRight { get; private set; }

        private PlayerController _playerController;
        private PlayerCombat _playerCombat;
        private Camera _camera;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _playerCombat = GetComponent<PlayerCombat>();
            _camera = Camera.main;

            if (!_playerController)
            {
                enabled = false;
                return;
            }

            _playerController.OnMoveForwardPressed += () => IsMovingForward = true;
            _playerController.OnMoveForwardReleased += () => IsMovingForward = false;
            _playerController.OnMoveBackPressed += () => IsMovingBack = true;
            _playerController.OnMoveBackReleased += () => IsMovingBack = false;
            _playerController.OnMoveLeftPressed += () => IsMovingLeft = true;
            _playerController.OnMoveLeftReleased += () => IsMovingLeft = false;
            _playerController.OnMoveRightPressed += () => IsMovingRight = true;
            _playerController.OnMoveRightReleased += () => IsMovingRight = false;
        }

        private void FixedUpdate()
        {
            if (!_playerCombat.IsFinishing())
            {
                RotateTowardsCamera();
                var cameraForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                var cameraRight = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;

                if (IsMovingForward)
                {
                    transform.position += cameraForward * (Speed * Time.fixedDeltaTime);
                }

                if (IsMovingBack)
                {
                    transform.position += -cameraForward * (Speed * Time.fixedDeltaTime);
                }

                if (IsMovingLeft)
                {
                    transform.position += -cameraRight * (Speed * Time.fixedDeltaTime);
                }

                if (IsMovingRight)
                {
                    transform.position += cameraRight * (Speed * Time.fixedDeltaTime);
                }
            }
        }

        /// <summary>
        /// Поворачивает объект игрока в сторону, куда смотрит камера, по горизонтальной плоскости.
        /// </summary>
        private void RotateTowardsCamera()
        {
            if (_camera)
            {
                Vector3 cameraForwardFlat = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
                if (cameraForwardFlat != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(cameraForwardFlat);
                }
            }
        }

        /// <summary>
        /// Возвращает, движется ли игрок в данный момент.
        /// </summary>
        public bool IsMoving()
        {
            return IsMovingForward || IsMovingBack || IsMovingLeft || IsMovingRight;
        }
    }
}