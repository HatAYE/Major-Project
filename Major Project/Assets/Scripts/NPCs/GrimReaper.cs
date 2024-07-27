using Conversa.Runtime;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GrimReaper : MonoBehaviour
{
    [SerializeField] int essences;
    Controls player;
    DialogueController controller;
    [SerializeField] Conversation firstConvo;
    [SerializeField] Conversation acceptConvo;
    [SerializeField] Conversation rejectConvo;
    [SerializeField] Conversation interactionConvo;
    [SerializeField] Conversation secondConvo;
    bool refused;
    bool accepted;
    bool canInteract;
    bool playerEnteredRadius;
    bool finishedFirstInteraction;

    [SerializeField] Volume postProcessVolume;
    Bloom bloomEffect;
    float target = 50f;
    float changeRate= 30;
    [SerializeField] GameObject essenceObject;

    PlayableDirector director;
    DialogueUI dialogueUI;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        controller = new DialogueController(firstConvo);
        controller.OnEventTrigger += CheckEssenceCount;
        controller.OnEventTrigger += TransitionToNextLevel;
        controller.OnEventTrigger += GiveEssence;
        controller.OnEventTrigger += ReaperLeaves;
        //  essences = FindObjectsOfType<Essence>().Length;
        dialogueUI=FindObjectOfType<DialogueUI>();
        #region post processing set up
        if (postProcessVolume != null)
        {
            if (postProcessVolume.profile.TryGet<Bloom>(out Bloom bloom))
            {
                bloomEffect = bloom;
            }
        }
        #endregion

        if (GetComponent<PlayableDirector>() != null) director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if(!finishedFirstInteraction)
        {
            StartInteraction();
        }
        
    }
    void CheckEssenceCount(string eventName)
    {
        if (eventName == "check essence")
        {
            if (player.essenceCollected >= essences)
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
                };
                canTransition = true;
            }
        }
    }
    bool gaveEssence;
    void GiveEssence(string eventName)
    {
        if (eventName == "give essence")
        {
            canInteract = false;
            StartCoroutine(ShowEssenceObject());
            StartCoroutine(player.AddEssence());

            controller.OnDialogueEnd += () =>
            {
                if (!gaveEssence)
                {
                    if (secondConvo != null)
                    {
                        controller.NewConversation(secondConvo);
                        controller.BeginDialogue();
                        //controller.OnDialogueEnd += () => canInteract = true;
                        gaveEssence = true;
                    }
                }
            };

        }
    }
    void ReaperLeaves(string eventName)
    {
        if (eventName == "leave")
        {
            StartCoroutine(LeaveAnimation());
        }
    }
    IEnumerator LeaveAnimation()
    {
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("leave");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    IEnumerator ShowEssenceObject()
    {
        if(essenceObject!=null)
        {
            essenceObject.SetActive(true);
            yield return new WaitForSeconds(6);
            essenceObject.SetActive(false);
        }
    }
    void ChangeEffects()
    {
        target = 0 + changeRate;
        StartCoroutine(AdjustEffects());
        
    }
    float time;
    float totalTime=4;
    IEnumerator AdjustEffects()
    {
        while (time < totalTime)
        {
            time += Time.deltaTime;
            bloomEffect.intensity.value = Mathf.Lerp(0, target, time / totalTime);
            yield return null;
        }
        time = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator SecondsPause(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (controller != null)
        {
            controller.BeginDialogue();
            finishedFirstInteraction = true;
        }
    }
    void StartInteraction()
    {
        if(playerEnteredRadius)
        {
            if(director!=null)
            {
                canInteract = false;
                GameManager.instance.ChangeState(gameStates.frozen);
                director.Play();
                dialogueUI.canSkipDialogue = false;
                StartCoroutine(SecondsPause(3));
            }
            else
            {
                if (canInteract == false)
                {
                    if (GameManager.instance.currentLevel == 1)
                    {
                        player.onEssenceCollection += () =>
                        {
                            StartFirstDialogue();
                        };
                    }
                    else StartFirstDialogue();
                    finishedFirstInteraction = true;
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

    void StartFirstDialogue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("appear");
        if (controller != null)
        {
            //play talking animation
            controller.BeginDialogue();
            controller.OnDialogueEnd += () => canInteract = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject== player.gameObject)
        {
            playerEnteredRadius = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            playerEnteredRadius = false;
        }
    }
}
