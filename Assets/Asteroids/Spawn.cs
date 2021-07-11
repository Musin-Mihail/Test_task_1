using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _sphereBig;
    public GameObject _field;
    Vector3 _spavnVector;
    public static Vector2 _minVector2;
    public static Vector2 _maxVector2;
    public static int _maxSpheres;
    public static int _spheresLeft;
    int check;
    void Start()
    {
        check = 0;
        _maxSpheres = 2;
        _minVector2 = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));
        _maxVector2 = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));
        float _sumX = _maxVector2.x + -_minVector2.x;
        float _sumY = _maxVector2.y + -_minVector2.y;
        _field.transform.localScale = new Vector3(_sumX - 0.5f,_sumY - 0.5f, 2);
        StartCoroutine(SpawnWhile(_maxSpheres));
    }
    void Update()
    {
        if(_spheresLeft == 0 && check == 1)
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
            _numberspheres--;
            yield return new WaitForSeconds(1.0f);
            SpawnSphere();
        }
        check = 1;
    }
    void SpawnSphere()
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
        GameObject _sphere = Instantiate(_sphereBig, _spavnVector, Quaternion.identity);
        Vector3 _jumpDirection = (Vector3.zero - _sphere.transform.position).normalized;
        _spheresLeft++;
        _sphere.GetComponent<Rigidbody>().AddForce(_jumpDirection*2, ForceMode.Impulse);
    }
}