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

    public AnimationClip[] IdleAnis= new AnimationClip[6];
    public AnimationClip[] MovementAnis = new AnimationClip[6];
    public AnimationClip[] JumpAnis = new AnimationClip[6];
    public AnimationClip[] CrouchAnis = new AnimationClip[6];
    public AnimationClip[] CrouchMovementAnis = new AnimationClip[6];
    public AnimationClip[] PushAndPullAnis = new AnimationClip[6];
    public AnimationClip[] CrouchPushAndPullAnis = new AnimationClip[6];
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

        UpdateCostume();

        if (GameManager.instance.currentLevel==1)
        {
            player.onEssenceCollection += EssenceCostumeChange;
        }
    }

    
    void UpdateCostume()
    {
        foreach (var stateItem in states)
        {
            /*if (stateItem.state.name == "Idle")
            {
                if (GameManager.instance.currentLevel==1)
                stateItem.state.motion = IdleAnis[0];
                
                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = IdleAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = IdleAnis[5];
            }


            else*/ if (stateItem.state.name == "Movement")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = MovementAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = MovementAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = MovementAnis[5];
            }


            else if (stateItem.state.name == "Jump")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = JumpAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = JumpAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = JumpAnis[5];
            }


            else if (stateItem.state.name == "Crouch")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = CrouchAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = CrouchAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = CrouchAnis[5];
            }


            else if (stateItem.state.name == "Crouch movement")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = CrouchMovementAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = CrouchMovementAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = CrouchMovementAnis[5];
            }


            /*else if (stateItem.state.name == "Push and pull")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = PushAndPullAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = PushAndPullAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = PushAndPullAnis[5];
            }


            else if (stateItem.state.name == "Crouch push and pull")
            {
                if (GameManager.instance.currentLevel == 1)
                    stateItem.state.motion = CrouchPushAndPullAnis[0];

                else if (GameManager.instance.currentLevel == 2)
                    stateItem.state.motion = CrouchPushAndPullAnis[6];

                else if (GameManager.instance.currentLevel >= 3)
                    stateItem.state.motion = CrouchPushAndPullAnis[5];
            }
            /*else if (stateItem.state.name == "Swinging")
            {
            if (GameManager.instance.currentLevel==1)
                stateItem.state.motion = PushAndPullAnis[0];
            }*/
        }
    }
    [SerializeField] int aniIndex=0;
    void EssenceCostumeChange()
    {
        if (aniIndex<5)
        {
            foreach (var stateItem in states)
            {
                /*if (stateItem.state.name == "Idle")
                {
                        stateItem.state.motion = IdleAnis[aniIndex];
                }

                else*/if (stateItem.state.name == "Movement")
                    {
                        stateItem.state.motion = MovementAnis[aniIndex];
                    }


                /*else if (stateItem.state.name == "Jump")
                {
                        stateItem.state.motion = JumpAnis[aniIndex];
                }*/


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
