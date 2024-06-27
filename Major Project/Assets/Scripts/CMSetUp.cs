using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CMSetUp : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera()
    {
        // Wait until the end of the frame to ensure everything is initialized
        yield return new WaitForEndOfFrame();

        var brain = Camera.main?.GetComponent<CinemachineBrain>();
        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        var cam = Camera.main;

        if (vcam != null)
        {
            vcam.m_Lens.OrthographicSize = 7;
        }

        if (cam != null)
        {
            cam.orthographicSize = 7;
        }
    }
}
