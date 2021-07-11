using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Rigidbody _rigidbody;
    void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player" && Player._immortality == 0)
        {
            Player._immortality = 2;
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if(other.gameObject.name == "UFO")
        {
            other.gameObject.GetComponent<UFO>().Death();
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if(other.gameObject.tag == "Bullet" )
        {
            if(gameObject.tag == "SphereBig")
            {
                Player._score += 20;
                SearchSphere(Spawn._spheresMediumList);
            }
            else if(gameObject.tag == "SphereMedium")
            {
                Player._score += 50;
                SearchSphere(Spawn._spheresSmallList);
            }
            else if(gameObject.tag == "SphereMedium")
            {
                Player._score += 100;
            }
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if (other.gameObject.tag == "EnemyBullet" )
        {
            if(gameObject.tag == "SphereBig")
            {
                SearchSphere(Spawn._spheresMediumList);
            }
            else if(gameObject.tag == "SphereMedium")
            {
                SearchSphere(Spawn._spheresSmallList);
            }
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
    }
    void SearchSphere(List<GameObject> _list)
    {
        float _speed = _rigidbody.velocity.magnitude;
        foreach (var item in _list)
        {
            if(!item.activeSelf)
            {
                item.transform.position = transform.position;
                item.transform.rotation = transform.rotation;
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.transform.Rotate(45.0f, 0.0f, 0.0f, Space.Self);
                item.SetActive(true);
                item.GetComponent<Rigidbody>().AddForce(item.transform.forward * _speed, ForceMode.Impulse);
                Spawn._spheresLeft++;
                break;
            }
        }
        foreach (var item in _list)
        {
            if(!item.activeSelf)
            {
                item.transform.position = transform.position;
                item.transform.rotation = transform.rotation;
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.transform.Rotate(-45.0f, 0.0f, 0.0f, Space.Self);
                item.SetActive(true);
                item.GetComponent<Rigidbody>().AddForce(item.transform.forward * _speed, ForceMode.Impulse);
                Spawn._spheresLeft++;
                break;
            }
        }
    }
}