using UnityEngine;

/// <summary>
/// This script inherits from Interactable, and sets a bool in the animator to be true when its Interact method is called. The name of the bool is a serialized field.
/// </summary>

public class AnimatedInteractable : Interactable
{
    [SerializeField] Animator animator;
    [SerializeField] string animatorBoolName;

    private void Start()
    {
        if (animatorBoolName == null) 
        {
            Debug.LogError("No bool name.");
        }
    }

    protected override void Interact()
    {
        if (animator == null || animatorBoolName == null) { return; }
        animator.SetBool(animatorBoolName, true);
    }
}
