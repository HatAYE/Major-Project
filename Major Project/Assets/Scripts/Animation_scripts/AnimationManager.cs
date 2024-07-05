using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance { get; set; }
    Controls player;
    Animator animator;
    [SerializeField] AnimatorOverrideController[] animatorOverrideController= new AnimatorOverrideController[5];
    public AnimationClip[] IdleAnis= new AnimationClip[5];
    public AnimationClip[] MovementAnis = new AnimationClip[5];
    public AnimationClip[] JumpAnis = new AnimationClip[5];
    public AnimationClip[] CrouchAnis = new AnimationClip[5];
    public AnimationClip[] CrouchMovementAnis = new AnimationClip[5];
    public AnimationClip[] HoldingObjectAnis = new AnimationClip[5];
    public AnimationClip[] PushAndPullAnis = new AnimationClip[5];
    public AnimationClip[] CrouchPushAndPullAnis = new AnimationClip[5];
    public AnimationClip[] CrouchPushAndPullMovementAnis = new AnimationClip[5];
    int aniIndex=0;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        player=FindObjectOfType<Controls>();
        animator = GetComponent<Animator>();
        //AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

        // states = animatorController.layers[0].stateMachine.states;
        //animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;

        if (GameManager.instance.currentLevel==1)
        {
            SetCostume(0);
            player.onEssenceCollection += EssenceCostumeChange;
        }
        else
        {
            SetCostume(4);
            player.onEssenceCollection -= EssenceCostumeChange;
        }
    }

    void SetCostume(int controllerNumber)
    {
        animator.runtimeAnimatorController=animatorOverrideController[controllerNumber];
    }

    /*void SetCostume(int spriteIndex)
    {
        List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        // Add animation clips to the override list
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(IdleAnis[0], IdleAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(MovementAnis[0], MovementAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(JumpAnis[0], JumpAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(CrouchAnis[0], CrouchAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(CrouchMovementAnis[0], CrouchMovementAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(HoldingObjectAnis[0], HoldingObjectAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(PushAndPullAnis[0], PushAndPullAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(CrouchPushAndPullAnis[0], CrouchPushAndPullAnis[spriteIndex]));
        overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(CrouchPushAndPullMovementAnis[0], CrouchPushAndPullMovementAnis[spriteIndex]));

        animatorOverrideController.ApplyOverrides(overrides);
    }*/
    void EssenceCostumeChange()
    {
        if (aniIndex<5)
        {
            SetCostume(aniIndex);
        }
        aniIndex++;
    }
}
