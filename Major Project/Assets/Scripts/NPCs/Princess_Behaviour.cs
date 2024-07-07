using Conversa.Demo.Scripts;
using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess_Behaviour : MonoBehaviour
{
    //princess will be in a singing state 
    //switch to dialogue when player is in radius
    //give player a heart
    //go back to singing, and deactivate the radius
    HealthSystem pl;
    bool gaveHeart;
    #region conversation
    [SerializeField] Conversation convo;
    DialogueController dialogueController;
    #endregion
    #region Music trail fade
    GameObject musicTrail;
    
    #endregion
    void Start()
    {
        pl = FindObjectOfType<HealthSystem>();
        dialogueController = new DialogueController(convo);
        musicTrail = transform.GetChild(0).gameObject;
        AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.princessSinging);
    }
    IEnumerator StopSinging()
    {
        StartCoroutine(musicTrail.GetComponent<MusicTrail>().FadeOutMusicTrails());
        StartCoroutine(AudioManager.Instance.FadeOut(gameObject.GetComponent<AudioSource>()));
        yield return new WaitForSeconds(1f);
    }
    IEnumerator ResumeSinging()
    {
        StartCoroutine(musicTrail.GetComponent<MusicTrail>().FadeInMusicTrails());
        StartCoroutine(AudioManager.Instance.FadeIn(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.princessSinging));
        yield return null;
        //PLAY SINGING/IDLE ANIMATION
    }

    IEnumerator TalkWithPlayer()
    {
        if (dialogueController != null)
        {
            dialogueController.BeginDialogue();
            dialogueController.OnDialogueEnd += () =>
            {
                if (!gaveHeart)
                {
                    pl.hp++;
                    pl.currentHealth++;
                    gaveHeart=true;
                    StartCoroutine(ResumeSinging());
                }
                
            };
        }
        yield return null;
    }
    IEnumerator StartInteraction()
    {
        yield return StartCoroutine(StopSinging());

        yield return StartCoroutine(TalkWithPlayer());

        gameObject.GetComponent<BoxCollider2D>().enabled = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject== pl.gameObject)
        {
            StartCoroutine(StartInteraction());
        }

    }

}
