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
        if (other.gameObject.tag == "Player" && Player._immortality == 0)
        {
            SoundExplosion();
            Player._immortality = 2;
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if (other.gameObject.name == "UFO")
        {
            SoundExplosion();
            other.gameObject.GetComponent<UFO>().Death();
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if (other.gameObject.tag == "Bullet")
        {
            if (gameObject.tag == "SphereBig")
            {
                SoundExplosion();
                Player._score += 20;
                SearchSphere(Spawn._spheresMediumList);
            }
            else if (gameObject.tag == "SphereMedium")
            {
                SoundExplosion();
                Player._score += 50;
                SearchSphere(Spawn._spheresSmallList);
            }
            else if (gameObject.tag == "SphereSmall")
            {
                SoundExplosion();
                Player._score += 100;
            }
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
        else if (other.gameObject.tag == "EnemyBullet")
        {
            SoundExplosion();
            if (gameObject.tag == "SphereBig")
            {
                SearchSphere(Spawn._spheresMediumList);
            }
            else if (gameObject.tag == "SphereMedium")
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
        foreach (var Sphere in _list)
        {
            if (!Sphere.activeSelf)
            {
                Sphere.transform.position = transform.position;
                Sphere.transform.rotation = transform.rotation;
                Rigidbody _rigidbodySphere = GetComponent<Rigidbody>();
                _rigidbodySphere.velocity = Vector3.zero;
                Sphere.transform.Rotate(45.0f, 0.0f, 0.0f, Space.Self);
                Sphere.SetActive(true);
                _rigidbodySphere.AddForce(Sphere.transform.forward * _speed, ForceMode.Impulse);
                Spawn._spheresLeft++;
                break;
            }
        }
        foreach (var Sphere in _list)
        {
            if (!Sphere.activeSelf)
            {
                Sphere.transform.position = transform.position;
                Sphere.transform.rotation = transform.rotation;
                Rigidbody _rigidbodySphere = GetComponent<Rigidbody>();
                _rigidbodySphere.velocity = Vector3.zero;
                Sphere.transform.Rotate(-45.0f, 0.0f, 0.0f, Space.Self);
                Sphere.SetActive(true);
                _rigidbodySphere.AddForce(Sphere.transform.forward * _speed, ForceMode.Impulse);
                Spawn._spheresLeft++;
                break;
            }
        }
    }
    void SoundExplosion()
    {
        if (gameObject.tag == "SphereBig")
        {
            Sound._sound = 2;
        }
        else if (gameObject.tag == "SphereMedium")
        {
            Sound._sound = 3;
        }
        else if (gameObject.tag == "SphereSmall")
        {
            Sound._sound = 4;
        }
    }
}