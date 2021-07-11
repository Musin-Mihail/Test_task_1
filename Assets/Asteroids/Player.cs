using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject _bullets;
    public GameObject _bulletPrefab;
    static Rigidbody _rigidbody;
    List<GameObject> _bulletsList;
    MeshRenderer _meshRenderer;
    int _numberBullets;
    public static int _immortality;
    public static int _score;
    public Text _scoreText;
    public List<GameObject> _lifeList;
    int _life;
    void Start()
    {
        _life = 3;
        _score = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
        _immortality = 0;
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
        _scoreText.text = _score.ToString();
        if(_immortality == 2)
        {
            if(_life > 0)
            {
                _immortality = 1;
                StartCoroutine(Immortality());
            }
            else
            {
                Time.timeScale = 0;
                _meshRenderer.enabled = false;
            }
        }
        if(Input.GetKey(KeyCode.W))
        {
            if(_rigidbody.velocity.magnitude > 6)
            {
                _rigidbody.velocity *= 0.99f;
            }
            else
            {
                _rigidbody.AddForce(transform.up/20, ForceMode.Impulse);
            }
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 0.0f, 2.0f, Space.Self);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 0.0f, -2.0f, Space.Self);
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
                        item.GetComponent<Rigidbody>().AddForce(transform.up*10, ForceMode.Impulse);
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
        Vector2 _newVector = new Vector2(Random.Range(Spawn._maxVector2.x, Spawn._minVector2.x), Random.Range(Spawn._maxVector2.y, Spawn._minVector2.y));
        transform.position = _newVector;
        _rigidbody.velocity = Vector3.zero;
        _life--;
        _lifeList[_life].SetActive(false);
        int count = 0;
        while(count != 3)
        {
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.25f);
            _meshRenderer.enabled = true;
            count++;
        }
        _immortality = 0;
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "EnemyBullet" && _immortality == 0)
        {
            _immortality = 2;
        }
    }
}