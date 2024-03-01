using System.Collections;
using UnityEngine;
using VInspector;

/// <summary>
/// Enables or disables a GameObject using the Interaction system, optionally allowing an animation to play beforehand.
/// </summary>

public class ToggleInteractable : Interactable
{
    [SerializeField] bool selfToggle;
    [HideIf(nameof(selfToggle))] 
    [SerializeField] GameObject objectToToggle; 
    [EndIf]
    
    [SerializeField] bool animated;
    [ShowIf(nameof(animated))]
    [SerializeField] string animatorBoolName;
    [SerializeField] string animationName;
    [SerializeField] Animator animator;
    [EndIf]

    protected override void Interact()
    {
        if (animated && animator != null)
        {
            StartCoroutine(AnimateThenToggle());
        }
        else
        {
            ToggleState();
        }
    }

    IEnumerator AnimateThenToggle()
    {
        animator.SetTrigger(animatorBoolName);
        AnimationClip clip = null;
        foreach (var animation in animator.runtimeAnimatorController.animationClips)
        {
            if (animation.name == animationName)
            {
                clip = animation;
                break;
            }
        }
        yield return new WaitForSeconds(clip.length);
        ToggleState();
    }

    void ToggleState()
    {
        if (selfToggle)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        else if (objectToToggle != null)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }
    }
}