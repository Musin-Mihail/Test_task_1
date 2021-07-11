using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBig : MonoBehaviour
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
            float _speed = _rigidbody.velocity.magnitude;
            foreach (var item in Spawn._spheresMediumList)
            {
                if(!item.activeSelf)
                {
                    item.SetActive(true);
                    item.transform.position = transform.position;
                    item.transform.rotation = transform.rotation;
                    item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    item.transform.Rotate(45.0f, 0.0f, 0.0f, Space.Self);
                    item.GetComponent<Rigidbody>().AddForce(item.transform.forward * _speed, ForceMode.Impulse);
                    Spawn._spheresLeft++;
                    break;
                }
            }
            foreach (var item in Spawn._spheresMediumList)
            {
                if(!item.activeSelf)
                {
                    item.SetActive(true);
                    item.transform.position = transform.position;
                    item.transform.rotation = transform.rotation;
                    item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    item.transform.Rotate(-45.0f, 0.0f, 0.0f, Space.Self);
                    item.GetComponent<Rigidbody>().AddForce(item.transform.forward * _speed, ForceMode.Impulse);
                    Spawn._spheresLeft++;
                    break;
                }
            }
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}