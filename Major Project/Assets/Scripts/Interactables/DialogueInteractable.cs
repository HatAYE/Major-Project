using Conversa.Runtime.Events;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    [SerializeField] DialogueController dialogueController;

    private void Start()
    {
        dialogueController = dialogueController == null ? GetComponent<DialogueController>() : dialogueController;
        if (dialogueController == null) 
        { 
            Debug.LogError($"No dialogue controller on {gameObject.name}."); 
        }
    }

    protected override void Interact()
    {
        dialogueController.BeginDialogue();
        interactionHandler.UnregisterInteractable(this);
        dialogueController.runner.OnConversationEvent.AddListener((e) => 
        { 
            if (e is EndEvent && !SingleInteraction) { ReRegister(); } 
        });
    }

    void ReRegister()
    {
        interactionHandler.RegisterInteractable(this);
    }
}
