using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _sphereBig;
    public GameObject _sphereMedium;
    public GameObject _sphereSmall;
    public Vector3 _spavnVector = new Vector3(11,11,0);
    public Vector3 _zeroVector = new Vector3(0,0,0);
    Vector2 min;
    Vector2 max;
    void Start()
    {
        min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0)); // bottom-left corner
        max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1)); // top-right corner
        // Debug.Log(min);
        // Debug.Log(max);
        StartCoroutine(SpawnWhile());
    }
    void Update()
    {
        
    }
    IEnumerator SpawnWhile()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            SpawnSphere();
        }
    }
    void SpawnSphere()
    {
        int _side = Random.Range(1,5); // 1 лево, 2 вверх, 3 право, 4 низ
        float _random = Random.Range(11.0f,-11.0f);
        if(_side == 1)
        {
            _spavnVector = new Vector3(min.x-1,_random, 0);
        }
        else if(_side == 2)
        {
            _spavnVector = new Vector3(_random,max.y+1, 0);
        }
        else if(_side == 3)
        {
            _spavnVector = new Vector3(max.x+1,_random, 0);
        }
        else if(_side == 4)
        {
            _spavnVector = new Vector3(_random,min.y-1, 0);
        }
        GameObject _sphere = Instantiate(_sphereBig, _spavnVector, Quaternion.identity);
        // Debug.Log(_side + " - " + _spavnVector);
        Vector3 _jumpDirection = ( Vector3.zero - _sphere.transform.position).normalized;
        _sphere.GetComponent<Rigidbody>().AddForce(_jumpDirection*2, ForceMode.Impulse);
    }
}