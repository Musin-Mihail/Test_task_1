using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject _bigAsteroidPrefab;
    public GameObject _mediumAsteroidPrefab;
    public GameObject _smallAsteroidPrefab;
    List<GameObject> _bigAsteroidsPool;
    List<GameObject> _mediumAsteroidsPool;
    List<GameObject> _smallAsteroidsPool;
    public GameObject _asteroidsParent;
    void Awake() 
    {
        Global.Pool = gameObject;
    }
    void Start()
    {
        _bigAsteroidsPool = new List<GameObject>();
        _mediumAsteroidsPool = new List<GameObject>();
        _smallAsteroidsPool = new List<GameObject>();
    }
    public void SearchFreeBigAsteroid(ref GameObject asteroid)
    {
        foreach (var freeAsteroid in _bigAsteroidsPool)
        {
            if (!freeAsteroid.activeSelf)
            {
                asteroid = freeAsteroid;
                return;
            }
        }
        _bigAsteroidsPool.Add(Instantiate(_bigAsteroidPrefab, transform.position, Quaternion.identity, _asteroidsParent.transform));
        asteroid = _bigAsteroidsPool[_bigAsteroidsPool.Count-1];
    }
    public void SearchFreeMediumAsteroid(ref GameObject asteroid)
    {
        foreach (var freeAsteroid in _mediumAsteroidsPool)
        {
            if (!freeAsteroid.activeSelf)
            {
                asteroid = freeAsteroid;
                return;
            }
        }
        _mediumAsteroidsPool.Add(Instantiate(_mediumAsteroidPrefab, transform.position, Quaternion.identity, _asteroidsParent.transform));
        asteroid = _mediumAsteroidsPool[_mediumAsteroidsPool.Count-1];
    }
    public void SearchFreeSmallAsteroid(ref GameObject asteroid)
    {
        foreach (var freeAsteroid in _smallAsteroidsPool)
        {
            if (!freeAsteroid.activeSelf)
            {
                asteroid = freeAsteroid;
                return;
            }
        }
        _smallAsteroidsPool.Add(Instantiate(_smallAsteroidPrefab, transform.position, Quaternion.identity, _asteroidsParent.transform));
        asteroid = _smallAsteroidsPool[_smallAsteroidsPool.Count-1];
    }
}