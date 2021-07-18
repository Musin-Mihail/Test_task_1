using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject _bulletsParent, _bulletPrefab;
    static Rigidbody _rigidbody;
    List<GameObject> _bulletsList;
    public List<GameObject> _lifeList;
    MeshRenderer _meshRenderer;
    bool _shoot;
    public static int _immortality, _score;
    public Text _scoreText;
    int _life, _layerMask = 1 << 8;
    RaycastHit _hit;
    Quaternion targetRotation;
    float _timeBullet;
    public AudioSource _thrustSoundEffect;
    void Start()
    {
        _shoot = true;
        Application.targetFrameRate = 60;
        _life = 3;
        _score = 0;
        _meshRenderer = GetComponent<MeshRenderer>();
        _immortality = 0;
        _bulletsList = new List<GameObject>();
        for (int i = 0; i < 40; i++)
        {
            GameObject _b = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, _bulletsParent.transform);
            _bulletsList.Add(_b);
        }
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(!_shoot)
        {
            _timeBullet += Time.deltaTime;
            if(_timeBullet > 0.33f)
            {
                _shoot = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _thrustSoundEffect.mute = false;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (!Input.GetKey(KeyCode.Mouse1))
            {
                _thrustSoundEffect.mute = true;
            }
        }
        _scoreText.text = _score.ToString();
        if (_immortality == 2)
        {
            if (_life > 0)
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
        if (Input.GetKey(KeyCode.W))
        {
            Forward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 0.0f, 2.0f, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 0.0f, -2.0f, Space.Self);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
        if (Settings._mouse == 1)
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
            {
                targetRotation = Quaternion.LookRotation(Vector3.forward, _hit.point - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.05f);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _thrustSoundEffect.mute = false;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (!Input.GetKey(KeyCode.W))
                {
                    _thrustSoundEffect.mute = true;
                }
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Forward();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shot();
            }
        }
    }
    public IEnumerator Immortality()
    {
        Vector2 _newVector = new Vector2(Random.Range(Global._maxVector2.x, Global._minVector2.x), Random.Range(Global._maxVector2.y, Global._minVector2.y));
        transform.position = _newVector;
        _rigidbody.velocity = Vector3.zero;
        _life--;
        _lifeList[_life].SetActive(false);
        int count = 0;
        while (count != 3)
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
        if (other.gameObject.tag == "EnemyBullet" && _immortality == 0)
        {
            _immortality = 2;
        }
    }
    void Shot()
    {
        if (_shoot)
        {
            foreach (var Bullet in _bulletsList)
            {
                if (!Bullet.activeSelf)
                {
                    Bullet.SetActive(true);
                    Bullet.transform.position = transform.position + transform.up;
                    Rigidbody _RigidbodyBullet = Bullet.GetComponent<Rigidbody>();
                    _RigidbodyBullet.velocity = Vector3.zero;
                    _RigidbodyBullet.AddForce(transform.up * 10, ForceMode.Impulse);
                    Sound._sound = 1;
                    _shoot = false;
                    _timeBullet = 0;
                    break;
                }
            }
        }
    }
    void Forward()
    {
        if (_rigidbody.velocity.magnitude > 6)
        {
            _rigidbody.velocity *= 0.99f;
        }
        else
        {
            _rigidbody.AddForce(transform.up / 20, ForceMode.Impulse);
        }
    }
}