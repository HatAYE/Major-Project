using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QueensInteraction : MonoBehaviour
{
    Controls player;
    DialogueController dialogueController;
    [SerializeField] Conversation startConvo;
    [SerializeField] Conversation rejectionConvo;
    [SerializeField] Conversation IceQueenRejection;

    [SerializeField] GameObject iceObject;
    [SerializeField] GameObject fireObject;
    [SerializeField] GameObject waterObject;

    [SerializeField] float timer;
    [SerializeField] float objectResetTimer = 100;
    GameObject currentObject;
    float distance;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        dialogueController = new DialogueController(startConvo);
        dialogueController.OnEventTrigger += GiveObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 5)
        {
            if (player.GetComponent<InteractionHandler>() != null)
            {
                player.GetComponent<InteractionHandler>().interactionUI.gameObject.SetActive(true);
                player.GetComponent<InteractionHandler>().interactionUI.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 1.4f, 0));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                player.GetComponent<InteractionHandler>().interactionUI.gameObject.SetActive(false);
                dialogueController.NewConversation(startConvo);
                dialogueController.BeginDialogue();
            }
        }
        else
        {
            if (player.GetComponent<InteractionHandler>() != null)
            {
                player.GetComponent<InteractionHandler>().interactionUI.gameObject.SetActive(false);
            }
        }
        if(currentObject != null&& currentObject.GetComponent<Pushable>()!=null)
        {
            if (currentObject.GetComponent<Pushable>().beingHeld == false)
            {
                if (timer < objectResetTimer)
                {
                    timer += 0.05f;
                }
                else
                {
                    Destroy(currentObject);
                    currentObject = null;
                    timer = 0;
                }
            }
            else
            {
                timer = 0;
            }
            
        }
        if(currentObject==null)
        {
            currentObject = null;
        }
    }
    bool rejected;
    void GiveObject(string eventName)
    {
        if(eventName== "requested ice")
        {
            if (currentObject==null)
            {
                currentObject= Instantiate(iceObject, transform.position, Quaternion.identity);
            }
            else
            {
                dialogueController.OnDialogueEnd += () =>
                {
                    if(!rejected)
                    {
                        dialogueController.NewConversation(rejectionConvo);
                        dialogueController.BeginDialogue();
                        dialogueController.OnDialogueEnd += () => rejected = true;
                    }
                };
            }
            rejected = false;
        }

        if (eventName == "requested flame")
        {
            if (currentObject == null)
            {
                currentObject = Instantiate(fireObject, transform.position, Quaternion.identity);
            }
            else
            {
                dialogueController.OnDialogueEnd += () =>
                {
                    if (!rejected)
                    {
                        dialogueController.NewConversation(rejectionConvo);
                        dialogueController.BeginDialogue();
                        dialogueController.OnDialogueEnd += () => rejected = true;
                    }
                };
            }
            rejected = false;
        }

        if (eventName == "requested water")
        {
            //dialogueController.OnDialogueEnd += () =>
            // {
            if (player.holdObject != null && player.holdObject.GetComponent<ElementType>() != null)
            {
                if (player.holdObject.GetComponent<ElementType>().objectElement == Element.Fire)
                {
                    Destroy(player.holdObject);
                    player.holdObject = null;
                    if (currentObject == null)
                    {
                        currentObject = Instantiate(waterObject, transform.position, Quaternion.identity);
                    }
                    else
                    {
                        dialogueController.OnDialogueEnd += () =>
                        {
                            if (!rejected)
                            {
                                dialogueController.NewConversation(rejectionConvo);
                                dialogueController.BeginDialogue();
                                dialogueController.OnDialogueEnd += () => rejected = true;
                            }
                        };
                    }

                }

            }
            else
            {
                dialogueController.OnDialogueEnd += () =>
                {
                    if (!rejected)
                    {
                        dialogueController.NewConversation(IceQueenRejection);
                        dialogueController.BeginDialogue();
                        dialogueController.OnDialogueEnd += () => rejected = true;
                    }
                };
                rejected = false;
            }

            // };
        }
    }

    /*void GiveFlame(string eventName)
    {
        if (eventName == "requested flame")
        {

        }
    }

    void GiveWater(string eventName)
    {
        if (eventName == "requested water")
        {
            dialogueController.OnDialogueEnd += () =>
            {
                if (player.holdObject != null)
                {
                    if (player.holdObject.GetComponent<ElementType>() != null&& player.holdObject.GetComponent<ElementType>().objectElement== Element.Ice)
                    {
                        //instantiate water
                    }
                }
            };
        }
    }*/

    void DestroyDitchedObject()
    {

    }
}
