using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Rigidbody _rigidbody;
    public GameObject _pool;
    GameObject _spawnAsteroid;
    Pool Poolscript;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Poolscript = Global.Pool.GetComponent<Pool>();
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
        else if (other.gameObject.GetComponent<PlayerBullet>() && other.gameObject.GetComponent<PlayerBullet>().enabled == true)
        {
            if (gameObject.tag == "SphereBig")
            {
                SoundExplosion();
                Player._score += 20;
                float _speed = Random.Range(1.0f, 3.0f);
                Poolscript.SearchFreeMediumAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, 45.0f);
                Poolscript.SearchFreeMediumAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, -45.0f);
            }
            else if (gameObject.tag == "SphereMedium")
            {
                SoundExplosion();
                Player._score += 50;
                float _speed = Random.Range(1.0f, 3.0f);
                Poolscript.SearchFreeSmallAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, 45.0f);
                Poolscript.SearchFreeSmallAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, -45.0f);
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
        else if (other.gameObject.GetComponent<EnemyBullet>() && other.gameObject.GetComponent<EnemyBullet>().enabled == true)
        {
            SoundExplosion();
            if (gameObject.tag == "SphereBig")
            {
                float _speed = Random.Range(1.0f, 3.0f);
                Poolscript.SearchFreeMediumAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, 45.0f);
                Poolscript.SearchFreeMediumAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, -45.0f);
            }
            else if (gameObject.tag == "SphereMedium")
            {
                float _speed = Random.Range(1.0f, 3.0f);
                Poolscript.SearchFreeSmallAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, 45.0f);
                Poolscript.SearchFreeSmallAsteroid(ref _spawnAsteroid);
                SpanwAsteroid(_spawnAsteroid, _speed, -45.0f);
            }
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Spawn._spheresLeft--;
        }
    }
    void SpanwAsteroid(GameObject asteroid, float speed, float side)
    {
        asteroid.transform.position = transform.position;
        asteroid.transform.rotation = transform.rotation;
        Rigidbody _rigidbodySphere = asteroid.GetComponent<Rigidbody>();
        _rigidbodySphere.velocity = Vector3.zero;
        asteroid.transform.Rotate(side, 0.0f, 0.0f, Space.Self);
        asteroid.SetActive(true);
        _rigidbodySphere.AddForce(asteroid.transform.forward * speed, ForceMode.Impulse);
        Spawn._spheresLeft++;
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