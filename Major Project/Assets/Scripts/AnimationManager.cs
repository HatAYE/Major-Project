using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance { get; set; }
    Controls player;
    Animator animator;
    ChildAnimatorState[] states;

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
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

        states = animatorController.layers[0].stateMachine.states;


        if (GameManager.instance.currentLevel==1)
        {
            SetCostume(0);
            player.onEssenceCollection += EssenceCostumeChange;
        }
        else
        {
            SetCostume(5);
            player.onEssenceCollection -= EssenceCostumeChange;
        }
    }

    
    void SetCostume(int spriteIndex)
    {
        foreach (var stateItem in states)
        {
            if (stateItem.state.name == "Idle")
            {
                    stateItem.state.motion = IdleAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Movement")
            {
                    stateItem.state.motion = MovementAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Jump")
            {
                stateItem.state.motion = JumpAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Crouch")
            {
                    stateItem.state.motion = CrouchAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Crouch movement")
            {
                    stateItem.state.motion = CrouchMovementAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Crouch push and pull still")
            {
                stateItem.state.motion = CrouchPushAndPullAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Crouch push and pull movement")
            {
                stateItem.state.motion = CrouchPushAndPullMovementAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Push and Pull Movement")
            {
                    stateItem.state.motion = PushAndPullAnis[spriteIndex];
            }

            else if (stateItem.state.name == "Holding Object")
            {
                    stateItem.state.motion = HoldingObjectAnis[spriteIndex];
            }
            /*else if (stateItem.state.name == "Swinging")
            {
                stateItem.state.motion = PushAndPullAnis[spriteIndex];
            }*/
        }
    }
    void EssenceCostumeChange()
    {
        if (aniIndex<5)
        {
            SetCostume(aniIndex);
        }
        aniIndex++;
    }
}
