using Conversa.Runtime;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    [SerializeField] Conversation conversation;
    public DialogueController DialogueController { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DialogueController = new DialogueController(conversation);
    }

    protected override void Interact()
    {
        DialogueController.BeginDialogue();
        interactionHandler.UnregisterInteractable(this);
        if (!SingleInteraction) { DialogueController.OnDialogueEnd += ReRegister; }
    }

    void ReRegister()
    {
        interactionHandler.RegisterInteractable(this);
    }
}
