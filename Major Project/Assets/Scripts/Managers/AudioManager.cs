using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    AudioSource musicSource;
    AudioSource SFXSource;
    [SerializeField] AudioMixer audioMixer;

    float masterVolume = 1.0f;
    float musicFadeDuration = 1.0f;

    [Header (">>>       Levels soundtracks      <<<")]
    public AudioClip lvl1Soundtrack;
    public AudioClip lvl2Soundtrack;
    public AudioClip lvl3Soundtrack;
    public AudioClip lvl4Soundtrack;

    [Header(">>>      Menus      <<<")] 
    public AudioClip mainMenuAudio;

    [Header(">>>      Player Controls      <<<")]
    public AudioClip[] grassSoundEffects;
    public AudioClip jumpGrassSoundEffect;
    public AudioClip landGrassSoundEffect;
    public AudioClip[] woodSoundEffects;
    public AudioClip jumpWoodSoundEffects;
    public AudioClip landWoodSoundEffects;

    public AudioClip pushAndPullGrass;
    public AudioClip pushAndPullWood;

    public AudioClip crouchingSoundEffect;

    [Header(">>>      Princesses and sirens      <<<")] 
    public AudioClip princessSinging;
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
    void Start()
    {

        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        SFXSource= transform.GetChild(1).GetComponent<AudioSource>();

        if (GameManager.instance.currentLevel == 0)
        {
            PlaySound(musicSource, mainMenuAudio);
        }
        else if (GameManager.instance.currentLevel==1)
        {
            PlaySound(musicSource, lvl1Soundtrack);
        }
        else if (GameManager.instance.currentLevel==2)
        {
            PlaySound(musicSource, lvl2Soundtrack);
        }
        else if (GameManager.instance.currentLevel == 3)
        {
            PlaySound(musicSource, lvl3Soundtrack);
        }
        else if (GameManager.instance.currentLevel == 4)
        {
            PlaySound(musicSource, lvl4Soundtrack);
        }
    }

    void Update()
    {
        
    }

    public void SetMusicVolume(float level)
    {

    }

    public void SetSFXVolume(float level)
    {

    }

    public void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
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

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeDuration;
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
