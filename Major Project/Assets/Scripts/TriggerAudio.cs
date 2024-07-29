using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.extraSFXSource, audioClip);
        }
    }
}
