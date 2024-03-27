using UnityEngine;

/// <summary>
/// This is the abstract class used for Interactables. It contains an abstract method for interacting, and calls the Register/Unregister methods in the InteractionHandler class
/// when it enters/exits its trigger collider.
/// </summary>

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] bool singleInteraction;
    protected InteractionHandler interactionHandler;
    bool interacted;
    bool active = true;
    protected bool SingleInteraction { get => singleInteraction; }

    protected virtual void Awake()
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

    public void SetActive(bool value)
    {
        active = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.gameObject == interactionHandler.gameObject && !interacted)
        {
            interactionHandler.RegisterInteractable(this);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (active && collision.gameObject == interactionHandler.gameObject)
        {
            interactionHandler.UnregisterInteractable(this);
        }
    }

    private void OnDisable()
    {
        if (interactionHandler != null) interactionHandler.UnregisterInteractable(this);
    }
}