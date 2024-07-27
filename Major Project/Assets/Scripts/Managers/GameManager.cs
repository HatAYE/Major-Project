using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enemy;

public enum gameStates
{
    playing,
    paused,
    gameover,
    frozen,
    inDialogue,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    public static gameStates currenState;
    public int currentLevel;
    Controls pl;
    void Awake()
    {
        if (instance == null)
        {
            instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    public void Initialize()
    {
        pl = FindObjectOfType<Controls>();
        if (pl != null) pl.essenceCollected = 0;
        //currentLevel = SceneManager.GetActiveScene().buildIndex;
        AudioManager.Instance.CheckLevel();
        ChangeState(gameStates.playing);

    }

    void Update()
    {
        if (pl != null)
        {
            switch (currenState)
            {
                case gameStates.frozen:
                case gameStates.inDialogue:
                    pl.controlsAvaialble = false;
                    break;
                case gameStates.playing:
                    Time.timeScale = 1;
                    //pl.controlsAvaialble = true;
                    break;
                case gameStates.paused:
                case gameStates.gameover:
                    Time.timeScale = 0;
                    break;

                default:
                    break;
            }
        }
    }
    public void ChangeState(gameStates state)
    {
        currenState = state;
        if(state== gameStates.playing)
        {
            pl.controlsAvaialble = true;
        }
    }
}
