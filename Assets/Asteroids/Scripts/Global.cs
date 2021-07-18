using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static GameObject Pool;
    public static Vector2 _minVector2;
    public static Vector2 _maxVector2;
    public GameObject _field;
    public static float _maxDistans;
    void Start()
    {
        _minVector2 = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        _maxVector2 = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        float _sumX = _maxVector2.x + -_minVector2.x;
        float _sumY = _maxVector2.y + -_minVector2.y;
        if (_sumX > _sumY)
        {
            _maxDistans = _sumX;
        }
        else
        {
            _maxDistans = _sumY;
        }
        _field.transform.localScale = new Vector3(_sumX - 0.5f, _sumY - 0.5f, 2);
    }
}
