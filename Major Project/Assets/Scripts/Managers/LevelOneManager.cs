using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    float startingFocalStartingValue;
    float targetFocalLength;
    [SerializeField] float depthChangeRate;
    [SerializeField] float vignetteChangeRate;
    float vignetteStartingValue;
    float targetVignette;
    #endregion
    void Start()
    {
        pl=FindObjectOfType<Controls>();
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        #region post processing set up
        pl.onEssenceCollection += ChangeEffects;
        if (postProcessVolume!= null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);

            postProcessVolume.profile.TryGetSettings(out depth);
        }
        #endregion
        vignetteStartingValue= vignette.intensity.value;
        vignetteChangeRate = vignette.intensity.value / FindObjectsOfType<Essence>().Length;

        startingFocalStartingValue = depth.focalLength.value;
        depthChangeRate= depth.focalLength.value / FindObjectsOfType<Essence>().Length;
    }

    void Update()
    {  
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(pl.AddEssence());
        }*/
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(AdjustEffects());
        }
        if (scrappartText != null)
            scrappartText.text = scrapParts.ToString() +" scrap parts";
        
    }
    bool done;
    void ChangeEffects()
    {
        vignetteStartingValue = vignette.intensity.value;
        targetVignette = vignetteStartingValue - vignetteChangeRate;
        startingFocalStartingValue= depth.focalLength.value;
        targetFocalLength=startingFocalStartingValue - depthChangeRate;
        StartCoroutine(AdjustEffects());
    }
    float time;
    [SerializeField] float totalTime;
    IEnumerator AdjustEffects()
    {
        while(time<totalTime)
        {
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(vignetteStartingValue, targetVignette, time / totalTime);
            depth.focalLength.value = Mathf.Lerp(startingFocalStartingValue, targetFocalLength, time /totalTime);
            yield return null;
        }
        time = 0;
        
        //vignette.intensity.value = Mathf.Clamp01(initialIntensity - (pl.essenceCollected * 0.09f));
        //depth.focalLength.value = Mathf.Clamp(initialFocalLength - (pl.essenceCollected * 13), 20, initialFocalLength);
    }
}
