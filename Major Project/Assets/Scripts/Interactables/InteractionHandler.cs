using UnityEngine;

/// <summary>
/// This class is attached to the player, and handles initiating interactions with Interactables. It contains public methods for registering and unregistering
/// its current interactable object.
/// </summary>

public class InteractionHandler : MonoBehaviour
{
    Interactable currentInteractable;
    [SerializeField] KeyCode interactionButton = KeyCode.E;

    void Update()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionButton))
        {
            currentInteractable.TryInteraction();
        }
    }

    /// <summary>
    /// Registers an Interactable as the current interactable object. 
    /// </summary>
    /// <param name="interactable"> The Interactable to be registered.</param>
    public void RegisterInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    /// <summary>
    /// Unregisters an Interactable as the current interactable object. If the Interactable passed in does not match the current registered Interactable, nothing happens.
    /// </summary>
    /// <param name="interactable">The Interactable to be unregistered.</param>
    public void UnregisterInteractable(Interactable interactable)
    {
        if (currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }
}
