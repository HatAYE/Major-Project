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
