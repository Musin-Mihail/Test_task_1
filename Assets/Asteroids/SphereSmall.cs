using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSmall : MonoBehaviour
{
    Rigidbody _rigidbody;
    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Spawn._spheresLeft--;
            gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Bullet" )
        {
            Spawn._spheresLeft--;
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}