using Conversa.Runtime;
using System.Collections;
using UnityEngine;
public class SirenStateMachine : Enemy
{
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Conversation startingConvo;
    [SerializeField] Conversation endingConvo;
    DialogueController dialogueController = new DialogueController();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (playerInRadius)
        {
            StartCoroutine(EnemyBehavior());
            playerInRadius = false;
        }
    }

    IEnumerator EnemyBehavior()
    {
        yield return BeginDialogueCoroutine();
        
        yield return new WaitUntil(() => currentState == EnemyState.attack);

        // attack logic
        yield return AttackRoutine();

        // dialogue ending
        yield return FinalDialogueCoroutine();

        yield return new WaitUntil(() => currentState == EnemyState.die);
        // go home
        DieState();
        // any logic for resuming player control
    }

    protected override void IdleState()
    {
        //PLAY IDLE ANIMATION

        float amplitude = 0.002f;
        float speed = 1f;
        float initialY = transform.position.y;

        float newY = initialY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    IEnumerator BeginDialogueCoroutine()
    {
        if (dialogueController != null)
        {
            dialogueController.NewConversation(startingConvo);
            dialogueController.BeginDialogue();
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.attack);
            yield return new WaitForSeconds(1);
        }
    }

    bool attacked;
    protected override void AttackingState()
    {
        if (!attacked)
        {
            StartCoroutine(AttackRoutine());
            attacked = true;
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
        
        TransitionToState(EnemyState.dialogue2);
    }
    bool gaveHeart=false;
    IEnumerator FinalDialogueCoroutine()
    {
        if (dialogueController != null)
        {
            dialogueController.NewConversation(endingConvo);
             dialogueController.BeginDialogue();
            //PLAY ANIMATION OF SIREN GIVING A HEART
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.die);
            yield return new WaitForSeconds(1);
        }
        if (!gaveHeart)
        {
            player.GetComponent<HealthSystem>().hp++;
            gaveHeart = true;
        }
        yield return new WaitForSeconds(1.5f);
    }
    protected override void DieState()
    {
        //PLAY POOF ANIMATION
        Destroy(areaDetector);
        Destroy(gameObject);
    }
}
