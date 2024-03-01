using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to the player, and handles initiating interactions with Interactables. It contains public methods for registering and unregistering
/// its current interactable object.
/// </summary>

public class InteractionHandler : MonoBehaviour
{
    List<Interactable> registeredInteractables = new List<Interactable>();
    Interactable currentInteractable;
    [SerializeField] KeyCode interactionButton = KeyCode.E;
    [SerializeField] CanvasGroup interactionUI;
    [SerializeField] float UIFadeDuration;
    Coroutine fadeCoroutine;

    void Update()
    {
        if (registeredInteractables.Count > 0) { GetNearestInteractable(); }
        if (currentInteractable != null && Input.GetKeyDown(interactionButton)) { currentInteractable.TryInteraction(); }
    }

    /// <summary>
    /// Adds an Interactable to the list of registered Interactable objects. 
    /// </summary>
    /// <param name="interactable"> The Interactable to be registered.</param>
    public void RegisterInteractable(Interactable interactable)
    {
        registeredInteractables.Add(interactable);
        UpdateCurrentInteractable();
    }

    /// <summary>
    /// Removes an Interactable from the list of registered Interactable objects. 
    /// If the Interactable passed in is not in the list, nothing happens.
    /// </summary>
    /// <param name="interactable">The Interactable to be unregistered.</param>
    public void UnregisterInteractable(Interactable interactable)
    {
        if (registeredInteractables.Remove(interactable)) { UpdateCurrentInteractable(); }
    }

    void UpdateCurrentInteractable()
    {
        GetNearestInteractable();
        if (interactionUI == null) { return; }
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeUI(interactionUI.alpha, currentInteractable != null ? 1 : 0, UIFadeDuration * Mathf.Abs(1 - 2 * interactionUI.alpha)));
    }

    void GetNearestInteractable()
    {
        Interactable nearestInteractable = null;
        float nearestDistance = float.MaxValue;
        foreach (var interactable in registeredInteractables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInteractable = interactable;
            }
        }
        currentInteractable = nearestInteractable;
        if (interactionUI != null && currentInteractable != null) { interactionUI.transform.position = Camera.main.WorldToScreenPoint(currentInteractable.transform.position); }
    }

    IEnumerator FadeUI(float startAlpha, float targetAlpha, float duration)
    {
        float time = 0;
        if (!interactionUI.gameObject.activeSelf) { interactionUI.gameObject.SetActive(true); }
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            interactionUI.alpha = alpha;
            yield return null;
        }
        interactionUI.alpha = targetAlpha;
        if (targetAlpha == 0f) { interactionUI.gameObject.SetActive(false); }
    }
}