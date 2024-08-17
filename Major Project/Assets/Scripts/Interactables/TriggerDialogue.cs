using Cinemachine;
using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] Conversation conversation;
    [SerializeField] bool singleInteraction;
    [SerializeField] float pauseBeforeDialogue;
    DialogueController controller;
    [SerializeField] bool ENDNPC;
    void Start()
    {
        controller = new DialogueController(conversation);
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(pauseBeforeDialogue);
        if (controller != null)
        {
            controller.BeginDialogue();
            if (singleInteraction)
                controller.OnDialogueEnd += () => Destroy(gameObject);
            else
            {
                controller.OnDialogueEnd += () =>
                {
                    if (ENDNPC)
                    {
                        Controls player = FindObjectOfType<Controls>();
                        player.controlsAvaialble = false;
                    }
                };
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StartDialogue());
        }
    }
}
