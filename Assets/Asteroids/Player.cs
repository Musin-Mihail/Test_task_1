using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject _bullets;
    public GameObject _bulletPrefab;
    static Rigidbody _rigidbody;
    List<GameObject> _bulletsList;
    MeshRenderer _meshRenderer;
    int _numberBullets;
    public static int _immortality;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _immortality = 2;
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
        if(_immortality == 2)
        {
            _immortality = 1;
            StartCoroutine(Immortality());
        }
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
                        item.GetComponent<Rigidbody>().AddForce(transform.up*6, ForceMode.Impulse);
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
    public IEnumerator Immortality()
    {
        int test = 0;
        while(test != 3)
        {
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = true;
            test++;
        }
        _immortality = 0;
    }
}