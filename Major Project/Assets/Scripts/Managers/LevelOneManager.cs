using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LevelOneManager : MonoBehaviour
{
    Controls pl;
    static public int scrapParts;
    [SerializeField] Text scrappartText;
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
        #region post processing set up
        if (postProcessVolume!= null)
        {
            postProcessVolume = FindObjectOfType<PostProcessVolume>();
            postProcessVolume.profile.TryGetSettings(out vignette);
            postProcessVolume.profile.TryGetSettings(out depth);
        }
        if (vignette != null)
        initialIntensity = vignette.intensity.value;
        if (depth != null)
        initialFocalLength = depth.focalLength.value;
        #endregion
    }

    void Update()
    {
        //AdjustEffects();   
        if (Input.GetKeyDown(KeyCode.P))
        {
            scrapParts += 5;
        }
        if (scrappartText != null)
            scrappartText.text = scrapParts.ToString() +" scrap parts";
    }
    void AdjustEffects()
    {
        vignette.intensity.value = Mathf.Clamp01(initialIntensity - (pl.essenceCollected * 0.09f));
        depth.focalLength.value = Mathf.Clamp(initialFocalLength - (pl.essenceCollected * 13), 20, initialFocalLength);
    }
}
