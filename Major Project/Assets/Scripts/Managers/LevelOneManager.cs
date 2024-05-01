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
    [SerializeField] Vignette vignette;
    [SerializeField] DepthOfField depth;
    [SerializeField] PostProcessVolume postProcessVolume;
    [SerializeField] float vignetteChangeRate;
    [SerializeField] float speedChange;
    [SerializeField] float targetFocalLength;
    #endregion
    float startingValue;
    float targetVignette;
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
        startingValue= vignette.intensity.value;
        vignetteChangeRate = vignette.intensity.value / FindObjectsOfType<Essence>().Length;
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
        startingValue = vignette.intensity.value;
        targetVignette = startingValue - vignetteChangeRate;
        StartCoroutine(AdjustEffects());
    }
    float time;
    [SerializeField] float totalTime;
    IEnumerator AdjustEffects()
    {
        while(time<totalTime)
        {
            time += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startingValue, targetVignette, time / totalTime);
            yield return null;
        }
        time = 0;
        
        //vignette.intensity.value = Mathf.Clamp01(initialIntensity - (pl.essenceCollected * 0.09f));
        //depth.focalLength.value = Mathf.Clamp(initialFocalLength - (pl.essenceCollected * 13), 20, initialFocalLength);
    }
}
