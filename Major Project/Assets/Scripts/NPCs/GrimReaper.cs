using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimReaper : MonoBehaviour
{
    DialogueController controller;
    [SerializeField] Conversation convo;
    void Start()
    {
        controller = new DialogueController(convo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            print("beep");
            if (player.essenceCollected == 7)
            {
                //poof animation
                transform.GetChild(0).gameObject.SetActive(true);
                if (controller!=null)
                {
                    //play talking animation
                    controller.BeginDialogue();
                    //controller.OnDialogueEnd+= move to next scene
                    //play blinding light, next scene should start with player falling down a rabbit hole
                }
            }
        }
    }
}
