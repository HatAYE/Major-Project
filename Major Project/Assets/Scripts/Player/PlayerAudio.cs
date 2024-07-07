using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    Controls player;
     bool isCoroutineRunning = false;
    [SerializeField] float stepInterval = 0.5f;
    [SerializeField] LayerMask audioLayers;
    RaycastHit2D hit;
    [SerializeField] float rayLength = 3.0f;
    string groundLayer;
    Coroutine currentCoroutine;
    bool landed;
    void Start()
    {
        player=GetComponent<Controls>();
        if(GameManager.instance.currentLevel==2)
        {
            player.onEssenceCollection += UpdateSoundtrack;
        }
        else player.onEssenceCollection-= UpdateSoundtrack;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, audioLayers);
        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.blue);
        groundLayer = GroundLayer();

//        print(LayerMask.LayerToName(hit.collider.gameObject.layer));

        if (player.isMoving && !player.isHoldingObject)
        {
            if (!isCoroutineRunning)
                currentCoroutine=StartCoroutine(PlayFootstepSounds());
        }

        if(player.isMoving && player.isHoldingObject)
        {
            if (!isCoroutineRunning)
                currentCoroutine= StartCoroutine(PushingAudio());
        }


        if (player.isfalling)
        {
            if (!isCoroutineRunning)
                StartCoroutine(LandingAudio());
        }



    }
    
    IEnumerator LandingAudio()
    {
        isCoroutineRunning = true;
        if (!landed)
        {
            if (groundLayer == "wood")
            {
                if(GameManager.instance.currentLevel==1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.muffledLandWoodSoundEffects);

                else AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.landWoodSoundEffects);
            }

            else if (groundLayer == "grass")
            {
                if (GameManager.instance.currentLevel == 1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.muffledLandGrassSoundEffect);

                else AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.landGrassSoundEffect);
            }
            landed = true;
        }
        yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
        landed = false;
        isCoroutineRunning = false;
    }

    IEnumerator PushingAudio()
    {
        isCoroutineRunning = true;
        while (player.isHoldingObject && player.isMoving && !player.isfalling)
        {
            if (groundLayer == "wood")
            {
                if (GameManager.instance.currentLevel == 1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.muffledPushAndPullWood);

                else AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.pushAndPullWood);
            }

            else if (groundLayer == "grass")
            {
                if (GameManager.instance.currentLevel == 1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.muffledPushAndPullGrass);

                else AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.pushAndPullGrass);
            }

            yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
        }
        isCoroutineRunning = false;
    }

    IEnumerator PlayFootstepSounds()
    {
        isCoroutineRunning = true;
        while (player.isMoving && player.isHoldingObject == false && !player.isfalling)
        {
            if (groundLayer == "wood")
            {
                if (GameManager.instance.currentLevel == 1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.woodSoundEffects[Random.Range(0, AudioManager.Instance.muffledWoodSoundEffects.Length)]);

                else
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.woodSoundEffects[Random.Range(0, AudioManager.Instance.woodSoundEffects.Length)]);
            }
            if (groundLayer == "grass")
            {
                if (GameManager.instance.currentLevel == 1)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.muffledGrassSoundEffects.Length)]);

                else AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.grassSoundEffects[Random.Range(0, AudioManager.Instance.grassSoundEffects.Length)]);
            }

            yield return new WaitForSeconds(stepInterval + (AudioManager.Instance.SFXSource.clip != null ? AudioManager.Instance.SFXSource.clip.length : 0f));
        }
        isCoroutineRunning = false;
    }
    string GroundLayer()
    {
        if (hit.collider != null)
        {
            if (hit.collider != null)
            {
                return LayerMask.LayerToName(hit.collider.gameObject.layer);
            }
        }
        return "None";
    }
    int audioIndex;
    void UpdateSoundtrack()
    {
        if (audioIndex<5)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, AudioManager.Instance.lvl2Soundtrack[audioIndex]);
        }
        audioIndex++;
    }
}

