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
    frozen,
    inDialogue
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    static public gameStates currenState;
    Controls pl;
    bool isPaused;
    [SerializeField] GameObject pauseMenu;
    void Start()
    {
        pl = FindObjectOfType<Controls>();
        if (instance == null)
        {
            instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        TogglePause();
        switch(currenState)
        {
            case gameStates.frozen:
            case gameStates.inDialogue:
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
    public void ChangeState(gameStates state)
    {
        currenState = state;
    }
}
