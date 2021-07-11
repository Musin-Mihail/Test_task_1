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
            Spawn._spheresLeft--;
            Vector2 _newVector = new Vector2(Random.Range(Spawn._maxVector2.x, Spawn._minVector2.x), Random.Range(Spawn._maxVector2.y, Spawn._minVector2.y));
            other.gameObject.transform.position = _newVector;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Player._immortality = 2;
            gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Bullet" )
        {
            Spawn._spheresLeft--;
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
        }
        void SearchSphere(List<GameObject> _list)
        {
            float _speed = _rigidbody.velocity.magnitude;
            foreach (var item in _list)
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
            foreach (var item in _list)
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

        }
    }
}