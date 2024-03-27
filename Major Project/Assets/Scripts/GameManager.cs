using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Enemy;

public enum gameStates
{
    playing,
    paused,
    gameover,
    frozen
}
public class GameManager : MonoBehaviour
{
    static gameStates currenState;
    Controls pl;
    bool isPaused;
    [SerializeField] GameObject pauseMenu;
    void Start()
    {
        pl = FindObjectOfType<Controls>();
    }

    void Update()
    {
        TogglePause();
        switch(currenState)
        {
            case gameStates.frozen:
                pl.enabled = false;
                break;
            case gameStates.playing:
                Time.timeScale = 1;
                pl.enabled = true;
                break;
            case gameStates.paused:
            case gameStates.gameover:
                Time.timeScale = 0;
                break;

                default: 
                break;
        }
    }
    void TogglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            currenState= gameStates.paused;
            pauseMenu.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused= false;
            currenState = gameStates.playing;
            pauseMenu.gameObject.SetActive(false);
        }
    }
}
