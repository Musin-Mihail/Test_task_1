using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    // public enum OPTIONS{Menu,Asteroids}
    // public OPTIONS NameScene;
    public void LoadingScene(string NameScene)
    {
        SceneManager.LoadScene(NameScene.ToString());
    }
}