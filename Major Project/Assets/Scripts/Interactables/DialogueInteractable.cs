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
        if (!SingleInteraction) { dialogueController.OnDialogueEnd += ReRegister; }
    }

    void ReRegister()
    {
        interactionHandler.RegisterInteractable(this);
    }
}
