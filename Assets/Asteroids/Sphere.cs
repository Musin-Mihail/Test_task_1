using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.name == "Field" )
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Sphere" || other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}