using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    Controls player;

    public LayerMask woodLayer;
    public LayerMask grassLayer;
    [SerializeField] float stepInterval = 0.5f;
    bool isCoroutineRunning = false;
    bool pushingCroutineRunning;
    void Start()
    {
        player=GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isMoving && !isCoroutineRunning && player.isHoldingObject)
        {
            StartCoroutine(PlayFootstepSounds());
        }
        else if (!player.isMoving)
        {
            StopCoroutine(PlayFootstepSounds());
        }

        if(player.isHoldingObject && !pushingCroutineRunning)
        {
            StartCoroutine(PushingAudio());

        }

        if (Input.GetKeyDown(player.jump) && player.canjump == true)
        {
            Collider2D groundCollider = GetGroundCollider();

            if (woodLayer == (woodLayer | (1 << groundCollider.gameObject.layer)))
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.jumpWoodSoundEffects);
            }
            else if (grassLayer == (grassLayer | (1 << groundCollider.gameObject.layer)))
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.jumpGrassSoundEffect);
            }
        }

        IEnumerator PushingAudio()
        {
            pushingCroutineRunning = true;
            while (player.isHoldingObject)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.pushAndPullGrass);
                yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
            }
            pushingCroutineRunning = false;
        }

        IEnumerator PlayFootstepSounds()
        {
            isCoroutineRunning = true;
            while (player.isMoving)
            {
                Collider2D groundCollider = GetGroundCollider();

                if (groundCollider != null)
                {
                    if (woodLayer == (woodLayer | (1 << groundCollider.gameObject.layer)))
                    {
                        AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.woodSoundEffects[Random.Range(0, AudioManager.Instance.woodSoundEffects.Length)]);
                    }
                    else if (grassLayer == (grassLayer | (1 << groundCollider.gameObject.layer)))
                    {
                        AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)]);
                    }
                }

                yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
            }
            isCoroutineRunning = false;
        }

        Collider2D GetGroundCollider()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position * 2, Vector2.down, 2.5f);
            if (hit.collider != null)
            {
                return hit.collider;
            }

            return null;
        }

    }
}
