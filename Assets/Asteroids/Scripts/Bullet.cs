using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 _oldVector2;
    float _totalDistans;
    void OnEnable()
    {
        _totalDistans = 0;
        _oldVector2 = transform.position;
    }
    void OnDisable() 
    {
        GetComponent<PlayerBullet>().enabled = false;
        GetComponent<EnemyBullet>().enabled = false;
    }
    void Update()
    {
        float _distans = Vector2.Distance(_oldVector2,transform.position);
        if(_distans < 0.5f)
        {
            _totalDistans += _distans;
        }
        _oldVector2 = transform.position;
        if(_totalDistans > Global._maxDistans)
        {
            gameObject.SetActive(false);
        }
    }
}