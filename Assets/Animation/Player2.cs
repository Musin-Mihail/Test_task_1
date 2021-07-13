using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public Animator _animator;
    public int _moveForward;
    public int _moveBack;
    public int _moveLeft;
    public int _moveRight;
    public Transform targetBone;
    public Vector3 _Left;
    public Vector3 _Right;
    public Vector3 rot;
    // int check;
    void Start()
    {
        // check = 0;
    }
    void LateUpdate()
    {
        // targetBone.localEulerAngles = rot;
        // if(Input.GetKeyDown(KeyCode.W))
        // {
            // check++;
            // if(_moveBack == 0)
            // {
            //     _animator.Play("Run_Rifle");
            // }
        // }
        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     // check++;
            
        //     if(_moveRight == 0)
        //     {
        //         // rot = _Right;
                
        //         _animator.Play("Run_Left_Rifle");
        //     }
        // }
        // if(Input.GetKeyDown(KeyCode.D))
        // {
        //     // check++;
        //     if(_moveLeft == 0)
        //     {
        //         // rot = _Left;
        //         _animator.Play("Run_Right_Rifle");
        //     }
        // }
        // if(Input.GetKeyDown(KeyCode.S))
        // {
        //     // check++;
        //     if(_moveForward == 0)
        //     {
        //         _animator.Play("Back_Run_Rifle");
        //     }
        // }

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
                // targetBone.localEulerAngles = _Left;
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
                // targetBone.localEulerAngles = _Right;
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

        if(Input.GetKeyUp(KeyCode.W))
        {
            // check--;
            _moveForward = 0;
        } 
        if(Input.GetKeyUp(KeyCode.A))
        {
            // check--;
            _moveLeft = 0;
        } 
        if(Input.GetKeyUp(KeyCode.D))
        {
            // check--;
            _moveRight = 0;
        } 
        if(Input.GetKeyUp(KeyCode.S))
        {
            // check--;
            _moveBack = 0;
        } 

        if(_moveForward == 0 && _moveBack == 0 && _moveLeft == 0 && _moveRight == 0)
        {
            _animator.Play("Idle");
        }
    }
}