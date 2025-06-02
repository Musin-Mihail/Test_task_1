using System.Collections;
using UnityEngine;

namespace Animation.Scripts
{
    public class Player2 : MonoBehaviour
    {
        public Animator animator;
        private int _moveForward;
        private int _moveBack;
        private int _moveLeft;
        private int _moveRight;
        private int _finishing;
        public GameObject finishingText;
        public Transform chest;
        private Vector3 _mouseVector;
        private Vector3 _target;
        private readonly Vector3 _camera = new(6.0f, 10.0f, -6.0f);
        private const int LayerMask = 1 << 8;
        public GameObject enemy;
        public Animator enemyAnimator;
        private RaycastHit _hit;
        public GameObject gun;
        public GameObject sword;
        public Collider playercollider;
        private const int Speed = 10;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            Camera.main.transform.position = transform.position + _camera;
            if (_finishing == 1)
            {
                chest.rotation = Quaternion.LookRotation(Vector3.up, _target - chest.position);
                chest.transform.Rotate(-30, 90, 10);
                transform.rotation = Quaternion.LookRotation(Vector3.up, _target - transform.position);
                transform.transform.Rotate(270, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.identity;
                var _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMask))
                {
                    _mouseVector = _hit.point;
                    _mouseVector += transform.up * 2;
                    chest.rotation = Quaternion.LookRotation(Vector3.up, _mouseVector - chest.position);
                    chest.transform.Rotate(-30, 90, 0);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    if (_moveBack == 0)
                    {
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Rifle"))
                        {
                            animator.Play("Run_Rifle");
                        }

                        _moveForward = 1;
                        transform.position += transform.forward / Speed;
                    }
                }

                if (Input.GetKey(KeyCode.A))
                {
                    if (_moveRight == 0)
                    {
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Left_Rifle") && _moveForward == 0 && _moveBack == 0)
                        {
                            animator.Play("Run_Left_Rifle");
                        }

                        _moveLeft = 1;
                        transform.position += -transform.right / Speed;
                    }
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (_moveLeft == 0)
                    {
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Right_Rifle") && _moveForward == 0 && _moveBack == 0)
                        {
                            animator.Play("Run_Right_Rifle");
                        }

                        _moveRight = 1;
                        transform.position += transform.right / Speed;
                    }
                }

                if (Input.GetKey(KeyCode.S))
                {
                    if (_moveForward == 0)
                    {
                        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Back_Run_Rifle"))
                        {
                            animator.Play("Back_Run_Rifle");
                        }

                        _moveBack = 1;
                        transform.position += -transform.forward / Speed;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (finishingText.activeSelf)
                    {
                        _finishing = 1;
                        playercollider.enabled = false;
                        finishingText.SetActive(false);
                        animator.Play("Run_Rifle");
                        StartCoroutine(Finishing());
                    }
                }

                if (_moveForward == 0 && _moveBack == 0 && _moveLeft == 0 && _moveRight == 0 && _finishing == 0)
                {
                    animator.Play("Idle");
                }
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                _moveForward = 0;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                _moveLeft = 0;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                _moveRight = 0;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                _moveBack = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                _target = other.gameObject.transform.position;
                finishingText.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                finishingText.SetActive(false);
            }
        }

        private IEnumerator Finishing()
        {
            float _distans;
            _distans = Vector3.Distance(transform.position, _target);
            while (_distans > 2.5f)
            {
                transform.position += transform.forward / (Speed / 2);
                _distans = Vector3.Distance(transform.position, _target);
                yield return new WaitForSeconds(0.001f);
            }

            gun.SetActive(false);
            sword.SetActive(true);
            animator.Play("Finishing");
            yield return new WaitForSeconds(0.35f);
            enemyAnimator.enabled = false;
            yield return new WaitForSeconds(1.15f);
            gun.SetActive(true);
            sword.SetActive(false);
            _finishing = 0;
            yield return new WaitForSeconds(4.0f);
            enemy.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            while (true)
            {
                var target2 = Random.insideUnitCircle * 10;
                var NewTarget = new Vector3(target2.x + transform.position.x, 0, target2.y + transform.position.z);
                var _distans2 = Vector3.Distance(transform.position, NewTarget);
                if (_distans2 > 6)
                {
                    enemy.transform.position = NewTarget;
                    break;
                }

                yield return new WaitForSeconds(0.1f);
            }

            enemyAnimator.enabled = true;
            enemy.SetActive(true);
            playercollider.enabled = true;
        }
    }
}