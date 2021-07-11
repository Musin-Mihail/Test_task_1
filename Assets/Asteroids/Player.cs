using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject _bullets;
    public GameObject _bulletPrefab;
    static Rigidbody _rigidbody;
    List<GameObject> _bulletsList;
    Vector2 _newVector;
    float _second;
    int _numberBullets;
    void Start()
    {
        _numberBullets = 3;
        _bulletsList = new List<GameObject>();
        for (int i = 0; i < 40; i++)
        {
            GameObject _b = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            _bulletsList.Add(_b);
           _b.transform.parent = _bullets.transform;
        }
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            if(_rigidbody.velocity.magnitude > 6)
            {
                _rigidbody.velocity *= 0.99f;
            }
            else
            {
                _rigidbody.AddForce(transform.up/30, ForceMode.Impulse);
            }
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_numberBullets > 0)
            {
                foreach (var item in _bulletsList)
                {
                    if(!item.activeSelf)
                    {
                        item.SetActive(true);
                        item.transform.position = transform.position + transform.up;
                        item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        item.GetComponent<Rigidbody>().AddForce(transform.up*4, ForceMode.Impulse);
                        _numberBullets--;
                        Invoke("Reloading", 1.0f);
                        break;
                    }
                }
            }
        }
    }
    void Reloading()
    {
        _numberBullets++;
    }
}