using UnityEngine;

public class ChildNPC : MonoBehaviour
{
    Controls pl;
    DialogueController dialogueController;
    bool gaveEssence;

    void Start()
    {
        pl= FindObjectOfType<Controls>();
        dialogueController = GetComponent<DialogueInteractable>().DialogueController;
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
