using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelOneManager : MonoBehaviour
{
    Controls pl;
    static public int scrapParts;
    //[SerializeField] Text scrappartText;
    /*#region post processing
    Vignette vignette;
    DepthOfField depth;
    PostProcessVolume postProcessVolume;
    float startingFocalStartingValue;
    float targetFocalLength;
    [SerializeField] float depthChangeRate;
    [SerializeField] float vignetteChangeRate;
    float vignetteStartingValue;
    float targetVignette;
    #endregion*/

    Light2D pinlight;
    float startRadiusValue=5.5f;
    float targetRadiusValue;
    float radiusChangeRate = 5.5f;

    float startingIntensitiy = 1;
    float targetIntensitiy;
    float intensityChangeRate = 0.1f;

    void Start()
    {
        pl=FindObjectOfType<Controls>();
        //postProcessVolume = FindObjectOfType<PostProcessVolume>();
        pinlight = pl.GetComponentInChildren<Light2D>();
        #region post processing set up
        pl.onEssenceCollection += ChangeEffects;
        /*if (postProcessVolume!= null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);

            postProcessVolume.profile.TryGetSettings(out depth);
        }*/
        #endregion

        /*#region post processing setup
        //vignetteStartingValue = vignette.intensity.value;
        //vignetteChangeRate = vignette.intensity.value / FindObjectsOfType<Essence>().Length;

        //startingFocalStartingValue = depth.focalLength.value;
        //depthChangeRate= depth.focalLength.value / FindObjectsOfType<Essence>().Length;
        #endregion*/
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
        //if (scrappartText != null)
           // scrappartText.text = scrapParts.ToString() +" scrap parts";
        
    }
    void ChangeEffects()
    {
        //vignetteStartingValue = vignette.intensity.value;
        //targetVignette = vignetteStartingValue - vignetteChangeRate;
        //startingFocalStartingValue= depth.focalLength.value;
        //targetFocalLength=startingFocalStartingValue - depthChangeRate;

        startRadiusValue = pinlight.pointLightOuterRadius;
        targetRadiusValue = pinlight.pointLightOuterRadius + radiusChangeRate;

        startingIntensitiy = pinlight.intensity;
        targetIntensitiy = pinlight.intensity + intensityChangeRate;

        StartCoroutine(AdjustEffects());
    }
    float time;
    [SerializeField] float totalTime;
    IEnumerator AdjustEffects()
    {
        
        while(time<totalTime)
        {
            time += Time.deltaTime;
            //vignette.intensity.value = Mathf.Lerp(vignetteStartingValue, targetVignette, time / totalTime);
            //depth.focalLength.value = Mathf.Lerp(startingFocalStartingValue, targetFocalLength, time /totalTime);
            pinlight.pointLightOuterRadius = Mathf.Lerp(startRadiusValue, targetRadiusValue, time / totalTime);
            pinlight.intensity = Mathf.Lerp(startingIntensitiy, targetIntensitiy, time / totalTime);

            yield return null;
        }
        time = 0;
    }
}
