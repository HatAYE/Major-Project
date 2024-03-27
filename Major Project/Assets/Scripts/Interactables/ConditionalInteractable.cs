using UnityEngine;

public class ConditionalInteractable : Interactable
{
    [RequireType(typeof(IConditional)), SerializeField] MonoBehaviour conditional;
    [SerializeField] Interactable interactionIfTrue;
    [SerializeField] Interactable interactionIfFalse;

    private void Start()
    {
        interactionIfTrue.active = interactionIfFalse.active = false;
    }

    protected override void Interact()
    {
        if ((conditional as IConditional).Check())
        {
            interactionIfTrue.TryInteraction();
        }
        else
        {
            interactionIfFalse.TryInteraction();
        }
    }
}