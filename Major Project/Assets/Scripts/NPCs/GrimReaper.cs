using Conversa.Runtime;
using System.Collections;
using UnityEngine;
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
    bool refused;
    bool accepted;
    bool canInteract;
    bool playerEnteredRadius;

    [SerializeField] Volume postProcessVolume;
    Bloom bloomEffect;
    float target = 50f;
    float changeRate= 30;
    void Start()
    {
        player=FindObjectOfType<Controls>();
        controller = new DialogueController(firstConvo);
        controller.OnEventTrigger += CheckEssenceCount;
        controller.OnEventTrigger += TransitionToNextLevel;
      //  essences = FindObjectsOfType<Essence>().Length;

        #region post processing set up
        if (postProcessVolume != null)
        {
            if (postProcessVolume.profile.TryGet<Bloom>(out Bloom bloom))
            {
                bloomEffect = bloom;
            }
        }
        #endregion
    }

    void Update()
    {
        StartInteraction();
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

    void StartInteraction()
    {
        if(playerEnteredRadius)
        {
            if (canInteract == false)
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
   /* private void OnTriggerStay2D(Collider2D collision)
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
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject== player.gameObject)
        {
            playerEnteredRadius = true;
        }

    }
}
