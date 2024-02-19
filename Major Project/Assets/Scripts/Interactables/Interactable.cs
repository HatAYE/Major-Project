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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == interactionHandler.gameObject)
        {
            interactionHandler.RegisterInteractable(this);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject == interactionHandler.gameObject)
        {
            interactionHandler.UnregisterInteractable(this);
        }
    }
}