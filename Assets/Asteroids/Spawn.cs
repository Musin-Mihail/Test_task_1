using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _sphereBig;
    public GameObject _sphereMedium;
    public GameObject _sphereSmall;
    public GameObject _field;
    Vector3 _spavnVector;
    public static Vector2 _minVector2;
    public static Vector2 _maxVector2;
    public static int _maxSpheres;
    public static int _spheresLeft;
    public static float _maxDistans;
    int check;
    List<GameObject> _spheresList;
    public static List<GameObject> _spheresMediumList;
    public static List<GameObject> _spheresSmallList;
    public GameObject _asteroids;
    void Start()
    {
        _spheresLeft = 0;
        _spheresList = new List<GameObject>();
        _spheresMediumList = new List<GameObject>();
        _spheresSmallList = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            GameObject _a = Instantiate(_sphereBig, transform.position, Quaternion.identity);
            _spheresList.Add(_a);
            _a.transform.parent = _asteroids.transform;
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject _a = Instantiate(_sphereMedium, transform.position, Quaternion.identity);
            _spheresMediumList.Add(_a);
            _a.transform.parent = _asteroids.transform;
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject _a = Instantiate(_sphereSmall, transform.position, Quaternion.identity);
            _spheresSmallList.Add(_a);
            _a.transform.parent = _asteroids.transform;
        }
        check = 0;
        _maxSpheres = 2;
        _minVector2 = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));
        _maxVector2 = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));
        float _sumX = _maxVector2.x + -_minVector2.x;
        float _sumY = _maxVector2.y + -_minVector2.y;
        if(_sumX > _sumY)
        {
            _maxDistans = _sumX;
        }
        else
        {
            _maxDistans = _sumY;
        }
        _field.transform.localScale = new Vector3(_sumX - 0.5f,_sumY - 0.5f, 2);
        StartCoroutine(SpawnWhile(_maxSpheres));
    }
    void Update()
    {
        if(_spheresLeft <= 0 && check == 1)
        {
            check = 0;
            Spawn._maxSpheres++;
            StartCoroutine(SpawnWhile(_maxSpheres));
        }
    }
    public IEnumerator SpawnWhile(int _numberspheres)
    {
        while(_numberspheres != 0)
        {
            yield return new WaitForSeconds(1.0f);
            foreach (var item in _spheresList)
            {
                if(!item.activeSelf)
                {
                    SpawnSphere(item);
                    _numberspheres--;
                    break;
                }
            }
        }
        check = 1;
    }
    void SpawnSphere(GameObject _sphere)
    {
        int _side = Random.Range(1,5);
        float _random = Random.Range(11.0f,-11.0f);
        if(_side == 1)
        {
            _spavnVector = new Vector3(_minVector2.x-1,_random, 0);
        }
        else if(_side == 2)
        {
            _spavnVector = new Vector3(_random,_maxVector2.y+1, 0);
        }
        else if(_side == 3)
        {
            _spavnVector = new Vector3(_maxVector2.x+1,_random, 0);
        }
        else if(_side == 4)
        {
            _spavnVector = new Vector3(_random,_minVector2.y-1, 0);
        }
        _sphere.transform.position = _spavnVector;
        _sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _sphere.transform.LookAt(Vector3.zero);
        _sphere.SetActive(true);
        _sphere.GetComponent<Rigidbody>().AddForce(_sphere.transform.forward*Random.Range(1.0f,3.0f), ForceMode.Impulse);
        _spheresLeft++;
    }
}