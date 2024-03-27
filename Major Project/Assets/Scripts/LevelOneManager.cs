using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelOneManager : MonoBehaviour
{
    Controls pl;
    static public int scrapParts;

    #region post processing
    Vignette vignette;
    DepthOfField depth;
    PostProcessVolume postProcessVolume;
    float initialIntensity;
    float initialFocalLength;
    #endregion
    void Start()
    {
        pl=FindObjectOfType<Controls>();
        /*#region post processing set up
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out depth);
        initialIntensity = vignette.intensity.value;
        initialFocalLength = depth.focalLength.value;
        #endregion*/
    }

    void Update()
    {
        //AdjustEffects();   
        if (Input.GetKeyDown(KeyCode.P))
        {
            scrapParts += 5;
        }
    }
    void AdjustEffects()
    {
        vignette.intensity.value = Mathf.Clamp01(initialIntensity - (pl.essenceCollected * 0.5f));
        depth.focalLength.value = Mathf.Clamp(initialFocalLength - (pl.essenceCollected * 13), 20, initialFocalLength);
    }
}
