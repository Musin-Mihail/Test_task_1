using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Spawn._spheresLeft--;
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Bullet" )
        {
            Spawn._spheresLeft--;
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}