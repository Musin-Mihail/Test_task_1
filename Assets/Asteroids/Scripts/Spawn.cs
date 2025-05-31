using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    Vector3 _spawnVector;
    public static int _maxAsteroidsWave;
    public static int _spheresLeft;
    bool _startNewGame;
    GameObject _spawnAsteroid;
    void Start()
    {
        _spheresLeft = 0;
        _startNewGame = true;
        _maxAsteroidsWave = 2;
    }
    void Update()
    {
        if (_spheresLeft <= 0 && _startNewGame)
        {
            _startNewGame = false;
            StartCoroutine(StartNewWave(_maxAsteroidsWave));
            Spawn._maxAsteroidsWave++;
        }
    }
    public IEnumerator StartNewWave(int _numberAsteroids)
    {
        while (_numberAsteroids != 0)
        {
            yield return new WaitForSeconds(1.0f);
            Global.Pool.GetComponent<Pool>().SearchFreeBigAsteroid(ref _spawnAsteroid);  
            SpawnSphere(_spawnAsteroid);
            _numberAsteroids--;
        }
        _startNewGame = true;
    }
    void SpawnSphere(GameObject asteroid)
    {
        int _side = Random.Range(1, 5);
        float _random = Random.Range(11.0f, -11.0f);
        if (_side == 1)
        {
            _spawnVector = new Vector3(Global._minVector2.x - 1, _random, 0);
        }
        else if (_side == 2)
        {
            _spawnVector = new Vector3(_random, Global._maxVector2.y + 1, 0);
        }
        else if (_side == 3)
        {
            _spawnVector = new Vector3(Global._maxVector2.x + 1, _random, 0);
        }
        else if (_side == 4)
        {
            _spawnVector = new Vector3(_random, Global._minVector2.y - 1, 0);
        }
        asteroid.transform.position = _spawnVector;
        Rigidbody _rigidbodySphere = asteroid.GetComponent<Rigidbody>();
        _rigidbodySphere.linearVelocity = Vector3.zero;
        asteroid.transform.LookAt(Vector3.zero);
        asteroid.SetActive(true);
        _rigidbodySphere.AddForce(asteroid.transform.forward * Random.Range(1.0f, 3.0f), ForceMode.Impulse);
        _spheresLeft++;
    }
}