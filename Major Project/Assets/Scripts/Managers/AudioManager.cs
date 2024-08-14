using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using VInspector;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource extraSFXSource;
    [SerializeField] AudioMixer audioMixer;

    float musicFadeDuration = 1.0f;

    [Header (">>>       Levels soundtracks      <<<")]
    public AudioClip lvl1Soundtrack;
    public AudioClip[] lvl2Soundtrack;
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

    [SerializeField] bool muffled;
    [ShowIf(nameof(muffled))]
    [Header(">>>      Muffled soundeffects      <<<")]
    [SerializeField] public AudioClip[] muffledGrassSoundEffects;
    [SerializeField] public AudioClip muffledJumpGrassSoundEffect;
    [SerializeField] public AudioClip muffledLandGrassSoundEffect;
    [SerializeField] public AudioClip[] muffledWoodSoundEffects;
    [SerializeField] public AudioClip muffledJumpWoodSoundEffects;
    [SerializeField] public AudioClip muffledLandWoodSoundEffects;

    [SerializeField] public AudioClip muffledPushAndPullGrass;
    [SerializeField] public AudioClip muffledPushAndPullWood;

    [SerializeField] public AudioClip muffledCrouchingSoundEffect;
    [EndIf]

    [Header(">>>      Princesses and sirens      <<<")] 
    public AudioClip princessSinging;
    public AudioClip princessBattleMusic;

    [Header(">>>      Environment soundeffects      <<<")]
    public AudioClip respawnSoundeffect;
    public AudioClip platformBling1;
    public AudioClip platformBling2;
    public AudioClip essenceCollection;

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
        extraSFXSource = transform.GetChild(2).GetComponent<AudioSource>();
        CheckLevel();
    }
    public void CheckLevel()
    {
        if (GameManager.instance.currentLevel == 0)
        {
            PlaySound(musicSource, mainMenuAudio);
        }
        else if (GameManager.instance.currentLevel == 1|| GameManager.instance.currentLevel == 2)
        {
            PlaySound(musicSource, lvl1Soundtrack);
        }
        else if (GameManager.instance.currentLevel == 3)
        {
            PlaySound(musicSource, lvl3Soundtrack);
        }
        else if (GameManager.instance.currentLevel == 4)
        {
            PlaySound(musicSource, lvl4Soundtrack);
        }
        else if (GameManager.instance.currentLevel == 5)
        {
            musicSource.clip = null;
            SFXSource.clip = null;
            extraSFXSource.clip = null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
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
    /*public IEnumerator FadeTransition(AudioSource audioSource, AudioClip newClip, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }*/
}
