using Conversa.Demo.Scripts;
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
    [SerializeField] Conversation waterRejection;

    [SerializeField] GameObject iceObject;
    [SerializeField] GameObject fireObject;
    [SerializeField] GameObject waterObject;

    [SerializeField] float timer;
    [SerializeField] float objectResetTimer = 100;
    GameObject currentObject;
    GameObject currentFireObject;
    [SerializeField] bool canInteract;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        dialogueController = new DialogueController(startConvo);
        dialogueController.OnEventTrigger += GiveObject;
        canInteract = true;
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

            if (Input.GetKeyDown(KeyCode.E)&&canInteract)
            {
                canInteract = false;
                player.GetComponent<InteractionHandler>().interactionUI.gameObject.SetActive(false);
                dialogueController.NewConversation(startConvo);
                dialogueController.BeginDialogue();
                dialogueController.OnDialogueEnd += () => canInteract = true;
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
        if (currentFireObject == null) currentFireObject = null;
    }
    [SerializeField] bool rejected;
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
                rejected = false;
                
                dialogueController.OnDialogueEnd += () => StartCoroutine(Refuse(rejectionConvo));
            }
        }

        else if (eventName == "requested flame")
        {
            if (currentObject == null)
            {
                currentObject = Instantiate(fireObject, transform.position, Quaternion.identity);
            }
            else
            {
                rejected = false;
                dialogueController.OnDialogueEnd += () => StartCoroutine(Refuse(rejectionConvo));
            }

        }
        else if (eventName == "requested water")
        {
            if (player.holdObject != null && player.holdObject.GetComponent<ElementType>() != null && player.holdObject.GetComponent<ElementType>().objectElement == Element.Fire || currentFireObject != null)
            {
                if (currentFireObject != null)
                {
                    Destroy(currentFireObject);
                    currentFireObject = null;
                }
                else if (player.holdObject != null)
                {
                    Destroy(player.holdObject);
                    player.holdObject = null;
                }
                
                if (currentObject == null)
                {
                    currentObject = Instantiate(waterObject, transform.position, Quaternion.identity);
                }
                else
                {
                    rejected = false;
                    dialogueController.OnDialogueEnd += () => StartCoroutine(Refuse(rejectionConvo));
                }

            }
            else
            {
                rejected = false;
                dialogueController.OnDialogueEnd += () => StartCoroutine(Refuse(waterRejection));
            }
        }


    }
    IEnumerator Refuse(Conversation convo)
    {
        if (!rejected)
        {
            dialogueController.OnDialogueEnd = null;
            dialogueController.NewConversation(convo); 
            canInteract = false;
            dialogueController.BeginDialogue();
            canInteract = true;
            rejected = true;
            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ElementType obj))
        {
            if(obj.objectElement == Element.Fire)
            {
                currentFireObject = obj.gameObject;
            }
        }
    }
}
