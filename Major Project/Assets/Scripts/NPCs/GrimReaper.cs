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
        controller.OnEventTrigger += TransitionToNextLevel;
        
    }

    // Update is called once per frame
    void Update()
    {   
    }
    bool accepted;
    void CheckEssenceCount(string eventName)
    {
        print("mam");
        if (eventName == "check essence")
        {
            
            print("hello");
            if (player.essenceCollected == /*FindObjectsOfType<Essence>().Length*/7)
            {
                print("enough");
                controller.OnDialogueEnd += () => StartCoroutine(Accept());
            }
            else
            {
                print("not enough");
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

            controller.BeginDialogue();
            accepted=true;
            yield return null;
        }
        
    }
    bool refused;
    IEnumerator Refuse()
    {
        if (!refused)
        {
            controller.NewConversation(rejectConvo);
            controller.BeginDialogue();
            refused =true;

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
            if (player.essenceCollected >= 4)
            {
                player.onEssenceCollection += () =>
                {
                    //poof animation
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (controller != null)
                    {
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
