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
    public AnimationClip[] PushAndPullAnis = new AnimationClip[5];
    public AnimationClip[] CrouchPushAndPullAnis = new AnimationClip[5];
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
            player.onEssenceCollection += EssenceCostumeChange;
        }
        else
        {
            SetCostume();
            player.onEssenceCollection -= EssenceCostumeChange;
        }
    }

    
    void SetCostume()
    {
        foreach (var stateItem in states)
        {
            if (stateItem.state.name == "Idle")
            {
                    stateItem.state.motion = IdleAnis[5];
            }


            else if (stateItem.state.name == "Movement")
            {
                    stateItem.state.motion = MovementAnis[5];
            }


            else if (stateItem.state.name == "Jump")
            {
                stateItem.state.motion = JumpAnis[5];
            }


            else if (stateItem.state.name == "Crouch")
            {
                    stateItem.state.motion = CrouchAnis[5];
            }


            else if (stateItem.state.name == "Crouch movement")
            {
                    stateItem.state.motion = CrouchMovementAnis[5];
            }


            /*else if (stateItem.state.name == "Push and pull")
            {
                    stateItem.state.motion = PushAndPullAnis[5];
            }


            else if (stateItem.state.name == "Crouch push and pull")
            {
                    stateItem.state.motion = CrouchPushAndPullAnis[5];
            }
            /*else if (stateItem.state.name == "Swinging")
            {
                stateItem.state.motion = PushAndPullAnis[5];
            }*/
        }
    }
    int aniIndex=0;
    void EssenceCostumeChange()
    {
        if (aniIndex<5)
        {
            foreach (var stateItem in states)
            {
                if (stateItem.state.name == "Idle")
                {
                        stateItem.state.motion = IdleAnis[aniIndex];
                }

                else if (stateItem.state.name == "Movement")
                {
                        stateItem.state.motion = MovementAnis[aniIndex];
                }


                else if (stateItem.state.name == "Jump")
                {
                        stateItem.state.motion = JumpAnis[aniIndex];
                }


                else if (stateItem.state.name == "Crouch")
                {
                    stateItem.state.motion = CrouchAnis[aniIndex];
                }


                else if (stateItem.state.name == "Crouch movement")
                {
                    stateItem.state.motion = CrouchMovementAnis[aniIndex];
                }


                /* else if (stateItem.state.name == "Push and pull")
                 {
                         stateItem.state.motion = PushAndPullAnis[aniIndex];
                 }


                 else if (stateItem.state.name == "Crouch push and pull")
                 {
                         stateItem.state.motion = CrouchPushAndPullAnis[aniIndex];
                 }
                 /*else if (stateItem.state.name == "Swinging")
                 {
                     stateItem.state.motion = PushAndPullAnis[aniIndex];
                 }*/
            }
        }
            aniIndex++;
    }
    void Update()
    {
        
    }
}
