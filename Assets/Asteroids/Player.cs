using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Rigidbody _rigidbody;
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
}