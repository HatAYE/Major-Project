using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField] bool canInteract;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        controller = new DialogueController(firstConvo);
        controller.OnEventTrigger += CheckEssenceCount;
        controller.OnEventTrigger += TransitionToNextLevel;
        essences = FindObjectsOfType<Essence>().Length;
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
                print(":D");
                controller.OnDialogueEnd += () =>
                {
                    //transitiion to lvl 2;
                    print("it works");
                };
                canTransition = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            if (canInteract==false)
            {
                player.onEssenceCollection += () =>
                {
                    //poof animation
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (controller != null)
                    {
                        //play talking animation
                        controller.BeginDialogue();
                        controller.OnDialogueEnd += () => canInteract = true;
                        //controller.OnDialogueEnd+= move to next scene
                        //play blinding light, next scene should start with player falling down a rabbit hole
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
