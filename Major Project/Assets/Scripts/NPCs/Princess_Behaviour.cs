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
    [SerializeField] float fadeDuration = 2f; 
    Renderer rendererComponent;
    Material material;
    #endregion
    void Start()
    {
        pl = FindObjectOfType<HealthSystem>();
        dialogueController = new DialogueController(convo);
        musicTrail = transform.GetChild(0).gameObject;
        rendererComponent = musicTrail.GetComponent<Renderer>();

        if (rendererComponent != null)
        {
            material = rendererComponent.material;
        }
    }
    IEnumerator StopSinging()
    {
        Color color = material.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            color.a = alpha;
            material.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 0f;
        material.color = color;

        //FADE OUT AUDIO

        musicTrail.SetActive(false);
    }
    IEnumerator ResumeSinging()
    {
        musicTrail.SetActive(true);
        //FADE IN AUDIO 
        //PLAY SINGING/IDLE ANIMATION
        Color color = material.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            color.a = alpha;
            material.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 1f;
        material.color = color;
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
