using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Scripting;

public class GrimReaper : MonoBehaviour
{
    int essences;
    Controls player;
    DialogueController controller;
    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation acceptConvo;
    [SerializeField] Conversation rejectConvo;
    [SerializeField] Conversation interactionConvo;
    bool refused;
    bool accepted;
    bool canInteract;

    PostProcessVolume postProcessVolume;
    Bloom bloom;
    float target = 50f;
    float changeRate=70;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        controller = new DialogueController(firstConvo);
        controller.OnEventTrigger += CheckEssenceCount;
        controller.OnEventTrigger += TransitionToNextLevel;
        essences = FindObjectsOfType<Essence>().Length;

        #region post processing set up
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out bloom);
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {   
    }
    void CheckEssenceCount(string eventName)
    {
        if (eventName == "check essence")
        {
            if (player.essenceCollected == essences)
            {
                controller.OnDialogueEnd += () => StartCoroutine(Accept());
            }
            else
            {
                refused = false;
                controller.OnDialogueEnd += () => StartCoroutine(Refuse());
            }
        }
    }
    IEnumerator Accept()
    {
        if (!accepted)
        {
            controller.NewConversation(acceptConvo);
            canInteract = false;
            controller.BeginDialogue();
            accepted=true;
            controller.OnDialogueEnd += () => canInteract = true;
            yield return null;
        }
        
    }
    IEnumerator Refuse()
    {
        if (!refused)
        {
            controller.NewConversation(rejectConvo);
            canInteract = false;
            controller.BeginDialogue();
            refused =true;
            controller.OnDialogueEnd += () => canInteract = true;
            yield return null;
        }
        
    }
    bool canTransition;
    void TransitionToNextLevel(string eventName)
    {
        if (eventName== "transition")
        {
            if (!canTransition)
            {
                controller.OnDialogueEnd += () =>
                {
                    ChangeEffects();
                    //transitiion to lvl 2;
                };
                canTransition = true;
            }
        }
    }

    void ChangeEffects()
    {
        target = 0 + changeRate;
        StartCoroutine(AdjustEffects());
        print("1");
    }
    float time;
    float totalTime=4;
    IEnumerator AdjustEffects()
    {
        while (time < totalTime)
        {
            time += Time.deltaTime;
            bloom.intensity.value = Mathf.Lerp(0, target, time / totalTime);
            yield return null;
        }
        time = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            if (canInteract==false)
            {
                player.onEssenceCollection += () =>
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).GetComponent<Animator>().SetTrigger("appear");
                    if (controller != null)
                    {
                        //play talking animation
                        controller.BeginDialogue();
                        controller.OnDialogueEnd += () => canInteract = true;
                    }
                };
            }
            else
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (controller != null)
                    {
                        controller.NewConversation(interactionConvo);
                        controller.BeginDialogue();
                    }
                }
            }
            
        }
    }
}
