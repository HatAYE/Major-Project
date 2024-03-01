using UnityEngine;

/// <summary>
/// This is the abstract class used for Interactables. It contains an abstract method for interacting, and calls the Register/Unregister methods in the InteractionHandler class
/// when it enters/exits its trigger collider.
/// </summary>

public abstract class Interactable : MonoBehaviour
{
    protected InteractionHandler interactionHandler;
    [SerializeField] bool singleInteraction;
    bool interacted;
    protected bool SingleInteraction { get; private set; }

    void Awake()
    {
        interactionHandler = FindObjectOfType<InteractionHandler>();
    }

    public void TryInteraction()
    {
        if (singleInteraction && !interacted)
        {
            Interact();
            interacted = true;
        }
        else if (!singleInteraction)
        {
            Interact();
        }
    }

    /// <summary>
    /// The method to be overridden by every class that inherits from Interactable.
    /// </summary>
    protected abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == interactionHandler.gameObject)
        {
            interactionHandler.RegisterInteractable(this);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == interactionHandler.gameObject)
        {
            interactionHandler.UnregisterInteractable(this);
        }
    }

    private void OnDisable()
    {
        interactionHandler?.UnregisterInteractable(this);
    }
}