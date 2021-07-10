using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Rigidbody _rigidbody;
    Vector2 _newVector;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddForce(transform.up/30, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
        }
    }
    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.name == "Field" )
        {
            _newVector = transform.position;
            if(_newVector.x <= Spawn._minVector2.x || _newVector.x >= Spawn._maxVector2.x)
            {
                _newVector.x = -_newVector.x;
            }
            if(_newVector.y <= Spawn._minVector2.y || _newVector.y >= Spawn._maxVector2.y)
            {
                _newVector.y = -_newVector.y;
            }
            transform.position = _newVector;
        }
    }
}
