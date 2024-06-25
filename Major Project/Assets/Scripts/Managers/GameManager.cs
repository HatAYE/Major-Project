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

    #region Pause menu variables
    bool isPaused;
    public GameObject pauseMenu;
    GameObject audioPauseMenu;
    GameObject pauseButtons;
    Button resumeButton;
    Button restartButton;
    Button exitButton;
    Button restartCheckPointButton;
    Button audioSettingsButton;
    Button audioBackButton;
    #endregion
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
        pauseMenu = GameObject.Find("Pause menu");
        audioPauseMenu = GameObject.Find("Audio settings buttons");
        pauseButtons = GameObject.Find("Pause buttons");
        if (pauseMenu != null)
        {
            AssignButtons();
            pauseMenu.SetActive(false);
            print("not active");
        }
        ChangeState(gameStates.playing);
        //currentLevel = SceneManager.GetActiveScene().buildIndex+1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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

    void AssignButtons()
    {
        resumeButton = GameObject.Find("resume").GetComponent<Button>();
        restartButton= GameObject.Find("Restart level").GetComponent<Button>();
        exitButton = GameObject.Find("Exit").GetComponent<Button>();
        //restartCheckPointButton = GameObject.Find("Restart from last checkpoint").GetComponent<Button>();
        //audioSettingsButton= GameObject.Find("Audio settings").GetComponent<Button>();
        //audioBackButton= GameObject.Find("Back audio button").GetComponent<Button>();

        resumeButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartLevel);
        exitButton.onClick.AddListener(ExitGame);
        //restartCheckPointButton.onClick.AddListener(pl.Respawn);
        //audioSettingsButton.onClick.AddListener(OpenAudioMenu);
        //audioBackButton.onClick.AddListener(ExitAudioMenu);
    }
    public void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;
            currenState = gameStates.paused;
            pauseMenu.gameObject.SetActive(true);
        }
        else if (isPaused)
        {
            isPaused = false;
            currenState = gameStates.playing;
            pauseMenu.gameObject.SetActive(false);
        }
    }
    public void RestartLevel()
    {
        TogglePause();
        Scene currentScene= SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void OpenAnotherMenu(GameObject closingObject, GameObject openingObject)
    {
        closingObject.SetActive(false);
        openingObject.SetActive(true);
    }
    public void OpenAudioMenu()
    {
        OpenAnotherMenu(pauseButtons, audioPauseMenu);
    }

    public void ExitAudioMenu()
    {
        OpenAnotherMenu(audioPauseMenu, pauseButtons);
    }
    public void ChangeState(gameStates state)
    {
        currenState = state;
    }
}
