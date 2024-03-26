using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelOneManager : MonoBehaviour
{
    //control vignette depending on how much fireflies are collected
    int scrapParts;
    Controls pl;

    Vignette vignette;
    PostProcessVolume postProcessVolume;
    void Start()
    {
        pl=FindObjectOfType<Controls>();
        postProcessVolume.profile.TryGetSettings(out vignette);

    }

    void Update()
    {
        AdjustVignette();   
    }
    void AdjustVignette()
    {
        // Calculate the new vignette intensity based on fireflies collected
        float intensity = Mathf.Clamp01(1.0f - (pl.essenceCollected * 0.1f));

        // Apply the new intensity to the Vignette effect
        vignette.intensity.value = intensity;
    }
}
