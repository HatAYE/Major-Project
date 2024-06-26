using Cinemachine;
using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] Conversation conversation;
    [SerializeField] bool singleInteraction;
    DialogueController controller;
    void Start()
    {
        controller = new DialogueController(conversation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller != null)
            {
                print("oka");
                controller.BeginDialogue();
                if (singleInteraction)
                controller.OnDialogueEnd += () => Destroy(gameObject);
            }
        }
    }
}
