using Conversa.Runtime;
using System.Collections;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Dialogue,
    Attacking,
    FinalDialogue,
    Die
}
public class SirenStateMachine : MonoBehaviour
{
    public EnemyState currentState;
    GameObject player;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Conversation startingConvo;
    [SerializeField] Conversation endingConvo;
    [SerializeField] DialogueController dialogueController;

    void Start()
    {
        TransitionToState(EnemyState.Idle);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Dialogue:
                DialogueState();
                break;
            case EnemyState.Attacking:
                AttackingState();
                break;
            case EnemyState.FinalDialogue:
                FinalDialogue();
                break;
            case EnemyState.Die:
                DieState(); 
                break;

            default:
                break;
        }
    }

    void IdleState()
    {
        //PLAY IDLE ANIMATION

        float amplitude = 0.002f;
        float speed = 1f;
        float initialY = transform.position.y;

        float newY = initialY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void DialogueState()
    {
        /*GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            //play door closing animation
        }*/

        StartCoroutine(BeginDialogue());
    }

    IEnumerator BeginDialogue()
    {
        if (dialogueController != null)
        {
            dialogueController.NewConversation(startingConvo);
            dialogueController.BeginDialogue();
            yield return new WaitForSeconds(1);
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.Attacking);
        }
    }

    bool attacked;
    void AttackingState()
    {
        if (!attacked)
        {
            StartCoroutine(AttackRoutine());
            attacked=true;
        }
    }
    IEnumerator AttackRoutine()
    {
        Vector2 attackDirection = player.transform.position;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1.5f);
            for (int j = 0; j < 3; j++)
            {
                GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);
                Vector2 targetDirection = ((Vector3) attackDirection - projectile.transform.position).normalized;

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                projectileRb.velocity = targetDirection * 8;
                projectile.GetComponent<Projectile>().parentEnemy = gameObject;

                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1);
        }
        
        TransitionToState(EnemyState.FinalDialogue);
    }
    bool gaveHeart=false;
    void FinalDialogue()
    {
        StartCoroutine(finalDialogue());
    }
    IEnumerator finalDialogue()
    {
        if (dialogueController != null)
        {
            dialogueController.NewConversation(endingConvo);
             dialogueController.BeginDialogue();
            //PLAY ANIMATION OF SIREN GIVING A HEART
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.Die);
            yield return new WaitForSeconds(1);
        }
        if (!gaveHeart)
        {
            player.GetComponent<HealthSystem>().hp++;
            gaveHeart = true;
        }
        //TransitionToState(EnemyState.Die);
    }
    void DieState()
    {
        //PLAY POOF ANIMATION
        Destroy(gameObject);
    }

    public void TransitionToState(EnemyState newState)
    {
        currentState = newState;
    }


    
    
}
