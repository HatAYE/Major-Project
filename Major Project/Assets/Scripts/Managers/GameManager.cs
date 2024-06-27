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
    inDialogue
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    static public gameStates currenState;
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
        /*var brain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        var vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        var cam= FindObjectOfType<Camera>();

        if (brain == null)
        {
            vcam.m_Lens.OrthographicSize = 8;
            cam.orthographicSize = 8;
        }*/
    }

    public void Initialize()
    {
        pl = FindObjectOfType<Controls>();
        ChangeState(gameStates.playing);
        //currentLevel = SceneManager.GetActiveScene().buildIndex+1;
    }

    void Update()
    {
        switch(currenState)
        {
            case gameStates.frozen:
            case gameStates.inDialogue:
                pl.controlsAvaialble = false;
                break;
            case gameStates.playing:
                Time.timeScale = 1;
                pl.controlsAvaialble = true;
                break;
            case gameStates.paused:
            case gameStates.gameover:
                Time.timeScale = 0;
                break;

                default: 
                break;
        }
    }
    public void ChangeState(gameStates state)
    {
        currenState = state;
    }
}
