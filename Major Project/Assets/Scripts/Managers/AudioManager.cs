using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    AudioSource source;

    public AudioClip princessSinging;
    float masterVolume = 1.0f;
    float musicFadeDuration = 1.0f;
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource == null)
        {
            source = GetComponent<AudioSource>();
        }
        audioSource.clip = audioClip;
        audioSource.volume = masterVolume;
        audioSource.Play();
    }
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }
    public IEnumerator FadeIn(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource == null) 
            source = GetComponent<AudioSource>();

        audioSource.volume = 0;
        audioSource.clip = audioClip;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += Time.deltaTime / musicFadeDuration;
            yield return null;
        }
        audioSource.volume = 1.0f;
    }
    public IEnumerator FadeOut(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        // Fade out gradually
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeDuration;
            yield return null;
        }

        // Ensure volume is set to 0
        audioSource.volume = 0;
        audioSource.Stop();
    }
}
