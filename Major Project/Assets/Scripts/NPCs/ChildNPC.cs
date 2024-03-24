using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildNPC : MonoBehaviour
{
    Controls pl;
    DialogueController dialogueController;
    bool gaveEssence;
    void Start()
    {
        pl= FindObjectOfType<Controls>();
        dialogueController = GetComponent<DialogueController>();
    }

    // Update is called once per frame
    void Update()
    {
        dialogueController.OnEventTrigger += SharedLight;
        dialogueController.OnEventTrigger += Refused;
    }
    void SharedLight(string eventName)
    {
        if (eventName== "shared light")
        {
            //play happy npc animation
            if (!gaveEssence)
            {
                pl.essenceCollected++;
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                gaveEssence = true;
            }
            
        }
    }

    void Refused(string eventName)
    {
        if (eventName== "refuse")
        {
            //play ded npc animation
            if (!gaveEssence)
            {
                pl.essenceCollected += 2;
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                gaveEssence = true;
            }
        }
    }
}
