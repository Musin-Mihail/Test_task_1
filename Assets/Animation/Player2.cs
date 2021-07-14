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
    Vector3 _camera = new Vector3(4.07f, 6.69f, -2.9f);
    int _layerMask  = 1 << 8;
    public GameObject _enemy;
    public GameObject _enemyGun;
    public Animator _enemyAnimator;
    RaycastHit _hit;
    void LateUpdate()
    {
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
                    transform.position += transform.forward/35;
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
                    transform.position += -transform.right/35;
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
                    transform.position += transform.right/35;
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
                    transform.position += -transform.forward/35;
                }
            }
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(_finishingText.activeSelf)
                {
                    StartCoroutine(Finishing());
                }         
            }
            if(_moveForward == 0 && _moveBack == 0 && _moveLeft == 0 && _moveRight == 0)
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
        _finishing = 1;
        _animator.Play("Run_Rifle");
        float _distans;
        _distans = Vector3.Distance(transform.position,_target);
        while(_distans > 2.5f)
        {
            transform.position += transform.forward/30;
            _distans = Vector3.Distance(transform.position,_target);
            yield return new WaitForSeconds(0.001f);
        }
        _animator.Play("Finishing");
        yield return new WaitForSeconds(1.5f);
        _finishing = 0;
    }
}