using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    bool canPlay;
    float timer;
    float maxTimer=100;
    bool startTimer;
    private void Start()
    {
        canPlay = true;
    }
    private void Update()
    {
        if(startTimer)
        {
            if (timer < maxTimer)
            {
                canPlay = false;
                timer += 0.5f;
            }
            else
            {
                timer = 0;
                canPlay = true;
                startTimer = false;
            }
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            startTimer = true;
            if (canPlay)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.extraSFXSource, audioClip);
                canPlay = false;
            }
        }
    }
}
