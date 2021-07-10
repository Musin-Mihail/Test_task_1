using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _sphereBig;
    public GameObject _sphereMedium;
    public GameObject _sphereSmall;
    public Vector3 _SpavnVector = new Vector3(11,11,0);
    void Start()
    {
        float _randomX = Random.Range(11.0f,-11.0f);
        float _randomY = Random.Range(11.0f,-11.0f);
        _SpavnVector = new Vector3(_randomX,_randomY, 0);
        Debug.Log(_randomX);
        Debug.Log(_randomY);
        // Instantiate(_sphereBig, new Vector3(i * 2.0F, 0, 0), Quaternion.identity);
    }
    void Update()
    {
        
    }
}
