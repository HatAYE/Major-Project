using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    static bool introPlayed;
    [SerializeField] GameObject intro;
    [SerializeField] GameObject mainButtonsGroup;
    [SerializeField] GameObject levelsButtonsGroup;
    void Start()
    {
        
        if (!introPlayed)
        {
            intro.gameObject.SetActive(true);
            introPlayed = true;
        }
        else
        {
            intro.gameObject.SetActive(false);
        }
        levelsButtonsGroup.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadProgress()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(lastScene, LoadSceneMode.Single);
    }

    public void PickLevel()
    {
        mainButtonsGroup.SetActive(false);
        levelsButtonsGroup.SetActive(true );
    }
    public void BackButton()
    {
        mainButtonsGroup.SetActive(true);
        levelsButtonsGroup.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
