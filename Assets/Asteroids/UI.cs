using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject _menuUI;
    public GameObject _scoreUI;
    public GameObject _game;
    public Button _continue;
    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!_menuUI.activeSelf)
            {
                Time.timeScale = 0;
                _scoreUI.SetActive(false);
                _menuUI.SetActive(true);
                _continue.interactable = true;
            }
        }
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Asteroids");
    }
    public void Continue()
    {
        Time.timeScale = 1;
        _scoreUI.SetActive(true);
        _menuUI.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}