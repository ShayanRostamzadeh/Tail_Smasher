using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePlayButton : MonoBehaviour
{
    //[HideInInspector] public bool isPressed = false;
    
    private Button pausePlay;
    public Button resume;
    public Button menu;

    private void Start()
    {
        pausePlay = GetComponent<Button>();
        pausePlay.onClick.AddListener(OnPressed_PausePlay);
        resume.onClick.AddListener(OnPressed_Resume);
        menu.onClick.AddListener(OnPressed_Menu);
    }

    private void OnPressed_PausePlay()
    {
        //isPressed = !isPressed;
        Time.timeScale = 0f;
        resume.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
    }

    private void OnPressed_Resume()
    {
        //isPressed = !isPressed;
        Time.timeScale = 1f;
        resume.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
    }

    private void OnPressed_Menu()
    {
        SceneManager.LoadScene("Enterance");
    }
}
