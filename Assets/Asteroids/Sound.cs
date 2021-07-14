using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource _AudioSource;
    public AudioClip _fireSoundEffect;
    public AudioClip _largeExplosion;
    public AudioClip _mediumExplosion;
    public AudioClip _smallExplosion;
    public AudioClip _thrustSoundEffect;
    public static int _sound;

    void Start() 
    {
        _AudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if( _sound == 1)
        {
            _sound = 0;
            _AudioSource.PlayOneShot(_fireSoundEffect, 0.3F);
        }
        if( _sound == 2)
        {
            _sound = 0;
            _AudioSource.PlayOneShot(_largeExplosion, 0.3F);
        }
        if( _sound == 3)
        {
            _sound = 0;
            _AudioSource.PlayOneShot(_mediumExplosion, 0.3F);
        }
        if( _sound == 4)
        {
            _sound = 0;
            _AudioSource.PlayOneShot(_smallExplosion, 0.3F);
        }
    }
}
