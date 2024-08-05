using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TriggerImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Conversation conversation;
    [SerializeField] float dialogueWait;
    RawImage memoryImage;
    PlayableDirector timeline;
    DialogueUI dialogueUI;
    DialogueController controller;
    void Start()
    {
        memoryImage= GameObject.Find("Memory image").GetComponent<RawImage>();
        timeline = GetComponent<PlayableDirector>();
        dialogueUI=FindObjectOfType<DialogueUI>();
        controller = new DialogueController(conversation);
        timeline.stopped += Deactivate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Controls player))
        {
            StartCoroutine(ShowImage());
        }
    }

    IEnumerator ShowImage()
    {
        GameManager.instance.ChangeState(gameStates.frozen);
        dialogueUI.canSkipDialogue = false;
        timeline.Play();
        yield return new WaitForSeconds(dialogueWait);
        controller.BeginDialogue();

    }

    void Deactivate(PlayableDirector director)
    {
        GameManager.instance.ChangeState(gameStates.playing);
        gameObject.SetActive(false);
    }
}
