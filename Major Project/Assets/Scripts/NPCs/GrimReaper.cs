using Conversa.Runtime;
using Conversa.Runtime.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrimReaper : MonoBehaviour
{
    Controls player;
    DialogueController controller;
    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation acceptConvo;
    [SerializeField] Conversation rejectConvo;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        controller = new DialogueController(firstConvo);
        controller.OnEventTrigger += CheckEssenceCount;

    }

    // Update is called once per frame
    void Update()
    {

    }
    void CheckEssenceCount(string eventName)
    {
        if (eventName== "check essence")
        {
            playerInRadius = true;
            if (player.essenceCollected == /*FindObjectsOfType<Essence>().Length*/7)
            {
                controller.OnDialogueEnd += () =>
                {
                    controller.NewConversation(acceptConvo);
                    controller.BeginDialogue();
                };
            }
            else
            {
                controller.OnEventTrigger += TransitionToNextLevel;
                controller.NewConversation(rejectConvo);
                controller.BeginDialogue();
            }
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
    [SerializeField] bool playerInRadius;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            if (player.essenceCollected >= 4 && !playerInRadius)
            {
                player.onEssenceCollection += () =>
                {
                    //poof animation
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (controller != null)
                    {
                        playerInRadius = true;
                        //play talking animation
                        controller.BeginDialogue();
                        //controller.OnDialogueEnd+= move to next scene
                        //play blinding light, next scene should start with player falling down a rabbit hole
                    }
                };
                
            }
        }
    }
}
