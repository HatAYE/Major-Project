using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public static GameSaver Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SaveProgress();
    }
    public void SaveProgress()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentSceneName);
        PlayerPrefs.Save();
        Debug.Log("Progress saved for scene: " + currentSceneName);

    }
}
