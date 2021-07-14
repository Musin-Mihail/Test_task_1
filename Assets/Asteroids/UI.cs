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
    public Image _keyboard;
    public Image _keyboardMouse;
    void Start()
    {
        Time.timeScale = 1;
        if(Settings._mouse == 0)
        {
            Keyboard();
        }
        else
        {
            KeyboardMouse();
        }
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
    public void Keyboard()
    {
        _keyboard.color = Color.green;
        _keyboardMouse.color = Color.white;
        Settings._mouse = 0;
    }
    public void KeyboardMouse()
    {
        _keyboard.color = Color.white;
        _keyboardMouse.color = Color.green;
        Settings._mouse = 1;
    }
}