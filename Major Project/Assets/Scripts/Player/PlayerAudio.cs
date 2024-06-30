using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    Controls player;

    /*public LayerMask woodLayer;
    public LayerMask grassLayer;
    bool isCoroutineRunning = false;
    bool pushingCroutineRunning;*/
    [SerializeField] float stepInterval = 0.5f;
    RaycastHit2D hit;
    [SerializeField] float rayLength = 3.0f;
    string groundLayer;
    void Start()
    {
        player=GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength);

        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.blue);
        groundLayer = GroundLayer();
        print(groundLayer);
        if (player.isMoving && !player.isHoldingObject)
        {
            if (groundLayer== "grass")
            {
                StartCoroutine(PlaySoundEffect(player.isMoving,AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)], stepInterval));
            }
            else if (groundLayer==  "wood")
            {
                StartCoroutine(PlaySoundEffect(player.isMoving, AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)], stepInterval));
            }
        }
        /*else if (!player.isMoving)
        {
            StopCoroutine(PlayFootstepSounds());
        }*/

        if(player.isMoving && player.isHoldingObject)
        {
            if (groundLayer == "grass")
            {
                StartCoroutine(PlaySoundEffect(player.isHoldingObject,AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)],stepInterval));
            }
            else if (groundLayer == "wood")
            {
                StartCoroutine(PlaySoundEffect(player.isHoldingObject, AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)],stepInterval));
            }
        }

        /*if (Input.GetKeyDown(player.jump) && player.canjump == true)
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
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.pushAndPullWood);
                yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
            }
            pushingCroutineRunning = false;
        }

        IEnumerator PlayFootstepSounds()
        {
            print("hr");
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
        }*/

    }
    IEnumerator PlaySoundEffect(bool condition, AudioSource source, AudioClip clip, float pauseInterval)
    {
        while (condition)
        {
            AudioManager.Instance.PlaySound(source, clip);
            yield return new WaitForSeconds(pauseInterval + (source.clip != null ? source.clip.length : 0f));
        }
    }
    string GroundLayer()
    {
        

        // Check if the raycast hit an object
        if (hit.collider != null)
        {
            return LayerMask.LayerToName(hit.collider.gameObject.layer);
        }

        return "None";
    }

    /*private void OnDrawGizmos()
    {
        Color color = Color.blue;

        Gizmos.DrawLine(transform.position, Vector2.down * rayLength);
    }*/
}
