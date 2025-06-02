using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    public Animator _animator;
    public int _moveForward;
    public int _moveBack;
    public int _moveLeft;
    public int _moveRight;
    public int _finishing;
    public GameObject _finishingText;
    public Transform _chest;
    public Vector3 _mouseVector;
    public Vector3 _target;
    Vector3 _camera = new Vector3(6.0f, 10.0f, -6.0f);
    int _layerMask  = 1 << 8;
    public GameObject _enemy;
    public GameObject _enemyGun;
    public Animator _enemyAnimator;
    RaycastHit _hit;
    public GameObject _gun;
    public GameObject _sword;
    public Collider _playercollider;
    int _speed = 10;
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        Camera.main.transform.position = transform.position + _camera;
        if(_finishing == 1)
        {
            _chest.rotation = Quaternion.LookRotation(Vector3.up, _target - _chest.position);
            _chest.transform.Rotate(-30,90,10);
            transform.rotation = Quaternion.LookRotation(Vector3.up, _target - transform.position);
            transform.transform.Rotate(270,180,0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                _mouseVector = _hit.point;
                _mouseVector += transform.up*2;
                _chest.rotation = Quaternion.LookRotation(Vector3.up, _mouseVector- _chest.position);
                _chest.transform.Rotate(-30,90,0);
            }
            if(Input.GetKey(KeyCode.W))
            {
                if(_moveBack == 0)
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Rifle"))
                    {
                        _animator.Play("Run_Rifle");
                    }
                    _moveForward = 1;
                    transform.position += transform.forward/_speed;
                }
            }
            if(Input.GetKey(KeyCode.A))
            {
                if(_moveRight == 0)
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Left_Rifle") && _moveForward == 0 && _moveBack == 0)
                    {
                        _animator.Play("Run_Left_Rifle");
                    }
                    _moveLeft = 1;
                    transform.position += -transform.right/_speed;
                }
            }
            if(Input.GetKey(KeyCode.D))
            {
                if(_moveLeft == 0)
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run_Right_Rifle") && _moveForward == 0 && _moveBack == 0)
                    {
                        _animator.Play("Run_Right_Rifle");
                    }
                    _moveRight = 1;
                    transform.position += transform.right/_speed;
                }
            }
            if(Input.GetKey(KeyCode.S))
            {
                if(_moveForward == 0)
                {
                    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Back_Run_Rifle"))
                    {
                        _animator.Play("Back_Run_Rifle");
                    }
                    _moveBack = 1;
                    transform.position += -transform.forward/_speed;
                }
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(_finishingText.activeSelf)
                {
                    _finishing = 1;
                    _playercollider.enabled = false;
                    _finishingText.SetActive(false);
                    _animator.Play("Run_Rifle");
                    StartCoroutine(Finishing());
                }         
            }
            if(_moveForward == 0 && _moveBack == 0 && _moveLeft == 0 && _moveRight == 0 && _finishing == 0)
            {
                _animator.Play("Idle");
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            _moveForward = 0;
        } 
        if(Input.GetKeyUp(KeyCode.A))
        {
            _moveLeft = 0;
        } 
        if(Input.GetKeyUp(KeyCode.D))
        {
            _moveRight = 0;
        } 
        if(Input.GetKeyUp(KeyCode.S))
        {
            _moveBack = 0;
        } 
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            _target = other.gameObject.transform.position;
            _finishingText.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Enemy")
        {
            _finishingText.SetActive(false);
        }
    }
    IEnumerator Finishing()
    {
        float _distans;
        _distans = Vector3.Distance(transform.position,_target);
        while(_distans > 2.5f)
        {
            transform.position += transform.forward/(_speed/2);
            _distans = Vector3.Distance(transform.position,_target);
            yield return new WaitForSeconds(0.001f);
        }
        _gun.SetActive(false);
        _sword.SetActive(true);
        _animator.Play("Finishing");
        yield return new WaitForSeconds(0.35f);
        _enemyAnimator.enabled = false;
        yield return new WaitForSeconds(1.15f);
        _gun.SetActive(true);
        _sword.SetActive(false);
        _finishing = 0;
        yield return new WaitForSeconds(4.0f);
        _enemy.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
            Vector2 target = Random.insideUnitCircle * 10;
            Vector3 NewTarget = new Vector3(target.x + transform.position.x, 0, target.y + transform.position.z);
            float _distans2 = Vector3.Distance(transform.position, NewTarget);
            if(_distans2 > 6)
            {
                _enemy.transform.position = NewTarget;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _enemyAnimator.enabled = true;
        _enemy.SetActive(true);
        _playercollider.enabled = true;
    }
}