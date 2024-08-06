using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngineInternal;

public class TriggerImage : MonoBehaviour
{
    [SerializeField] Sprite image;
    [SerializeField] Conversation conversation;
    [SerializeField] float dialogueWait;
    [SerializeField] RawImage memoryImage;
    [SerializeField] GameObject memoryCanvasObject;
    PlayableDirector timeline;
    DialogueController controller;
    bool addedEssence;
    void Start()
    {
        memoryCanvasObject.SetActive(false);
        timeline = GetComponent<PlayableDirector>();
        if(conversation!=null)
        controller = new DialogueController(conversation);
        timeline.stopped += Deactivate;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Controls player))
        {
            if(!addedEssence)
            {
                StartCoroutine(player.AddEssence());
                addedEssence = true; 
            }
            
            StartCoroutine(ShowImage());
        }
    }

    IEnumerator ShowImage()
    {
        memoryCanvasObject.SetActive(true);
        GameManager.instance.ChangeState(gameStates.frozen);
        memoryImage.texture = image.texture;
        timeline.Play();
        yield return new WaitForSeconds(dialogueWait);

    }

    void Deactivate(PlayableDirector director)
    {
        memoryCanvasObject.SetActive(false);
        if (conversation != null)
        {
            controller.BeginDialogue();
            controller.OnDialogueEnd += () =>
            {
                GameManager.instance.ChangeState(gameStates.playing);
                Destroy(gameObject);
            };

        }
        else
        {
            GameManager.instance.ChangeState(gameStates.playing);
            Destroy(gameObject);
        }
    }
}
