using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    float _time;
    float _random;
    int _life;
    Rigidbody _rigidbody;
    public GameObject _player;
    public AudioSource _largeFlyingSaucer;
    GameObject _spawnBullet;

    void Start()
    {
        _life = 0;
        _rigidbody = GetComponent<Rigidbody>();
        _random = Random.Range(20.0f, 40.0f);
    }
    void Update()
    {
        if (_life == 0)
        {
            _time += Time.deltaTime;
            if (_time > _random)
            {
                _life = 1;
                _largeFlyingSaucer.mute = false;
                StartCoroutine(Shooting());
                int _side = Random.Range(0, 2);
                if (_side == 0)
                {
                    transform.position = new Vector3(Global._minVector2.x - 1, Random.Range(Global._maxVector2.y * 0.8f, Global._minVector2.y * 0.8f), 0);
                    _rigidbody.AddForce(-transform.up * 5, ForceMode.Impulse);
                }
                else
                {
                    transform.position = new Vector3(Global._maxVector2.x + 1, Random.Range(Global._maxVector2.y * 0.8f, Global._minVector2.y * 0.8f), 0);
                    _rigidbody.AddForce(transform.up * 5, ForceMode.Impulse);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Field")
        {
            Death();
        }
    }
    IEnumerator Shooting()
    {
        while (_life == 1)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
            Sound._sound = 1;
            Global.Pool.GetComponent<Pool>().SearchFreeBullet(ref _spawnBullet);  
            _spawnBullet.SetActive(true);
            _spawnBullet.GetComponent<EnemyBullet>().enabled = true;
            _spawnBullet.GetComponent<MeshRenderer>().material.color = Color.red;
            _spawnBullet.transform.position = transform.position;
            Rigidbody _rigidbodyBullet = _spawnBullet.GetComponent<Rigidbody>();
            _rigidbodyBullet.velocity = Vector3.zero;
            Vector2 _vector = (_player.transform.position - transform.position).normalized;
            _rigidbodyBullet.AddForce(_vector * 10, ForceMode.Impulse);
        }
    }
    public void Death()
    {
        _largeFlyingSaucer.mute = true;
        _life = 0;
        _random = Random.Range(20.0f, 40.0f);
        _time = 0;
        _rigidbody.velocity = Vector3.zero;
        transform.position = new Vector3(100, 100, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBullet>())
        {
            if (other.gameObject.GetComponent<PlayerBullet>().enabled == true)
            {
                other.gameObject.SetActive(false);
                Player._score += 200;
                Death();
            }
        }
    }
}