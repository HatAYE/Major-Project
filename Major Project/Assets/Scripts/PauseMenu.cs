using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Controls pl;
    [SerializeField] bool isPaused;
    public GameObject pauseMenuObj;

    GameObject pauseButtons;
    Button resumeButton;
    Button restartButton;
    Button exitButton;
    Button restartCheckPointButton;

    #region audio buttons
    GameObject audioPauseMenu;
    Button audioSettingsButton;
    Button audioBackButton;

    Slider masterSlider;
    Slider musicSlider;
    Slider SFXSlider;
    #endregion
    void Start()
    {
        pl=FindObjectOfType<Controls>();
        audioPauseMenu = GameObject.Find("Audio settings buttons");
        pauseButtons = GameObject.Find("Pause buttons");
        if (pauseMenuObj != null)
        {
            AssignButtons();
            pauseMenuObj.SetActive(false);
            audioPauseMenu.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void AssignButtons()
    {
        resumeButton = GameObject.Find("resume").GetComponent<Button>();
        restartButton = GameObject.Find("Restart level").GetComponent<Button>();
        exitButton = GameObject.Find("Exit").GetComponent<Button>();
        restartCheckPointButton = GameObject.Find("Restart from last checkpoint").GetComponent<Button>();
        audioSettingsButton = GameObject.Find("Audio settings").GetComponent<Button>();
        audioBackButton= GameObject.Find("Back audio button").GetComponent<Button>();

        masterSlider= GameObject.Find("Master volume slider").GetComponent<Slider>();
        musicSlider= GameObject.Find("Music volume slider").GetComponent <Slider>();
        SFXSlider = GameObject.Find("SFX volume slider").GetComponent<Slider>();

        resumeButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartLevel);
        exitButton.onClick.AddListener(ExitGame);
        restartCheckPointButton.onClick.AddListener(pl.Respawn);
        audioSettingsButton.onClick.AddListener(OpenAudioMenu);
        audioBackButton.onClick.AddListener(ExitAudioMenu);

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }
    public void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;
            GameManager.instance.ChangeState(gameStates.paused);
            pauseMenuObj.gameObject.SetActive(true);
        }
        else if (isPaused)
        {
            isPaused = false;
            GameManager.instance.ChangeState(gameStates.playing);
            pauseMenuObj.gameObject.SetActive(false);
        }
    }
    public void RestartLevel()
    {
        TogglePause();
        Scene currentScene = SceneManager.GetActiveScene();
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
}
