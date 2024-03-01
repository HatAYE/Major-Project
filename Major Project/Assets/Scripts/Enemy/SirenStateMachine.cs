using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    void Start()
    {
        TransitionToState(EnemyState.Idle);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TransitionToState(EnemyState.Attacking);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            TransitionToState(EnemyState.FinalDialogue);
        }
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
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            //play door closing animation
        }

        //PLAY DIALOGUE
        //ONCE COMPLETED, TRANSITION TO ATTACKING
        TransitionToState(EnemyState.Attacking);
    }

    int i = 1;
    void AttackingState()
    {
        //THROW  3 PROJECTILES IN A STRAIGHT LINE 3 TIMES, PAUSING IN BETWEEN EACH
        //ONCE COMPLETED, TRANSITION TO FINAL DIALOGUE
         
        Vector2 attackDirection = player.transform.position;
        if (i==1)
        {
            StartCoroutine(AttackRoutine(attackDirection));
            i--;
        }
    }
    bool gaveHeart=false;
    void FinalDialogue()
    {
        //PLAY DIALOGUE
        //PLAY ANIMATION OF SIREN GIVING A HEART
        //GIVE PLAYER A HEART
        if (!gaveHeart)
        {
            player.GetComponent<HealthSystem>().hp++;
            gaveHeart = true;
        }
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


    [SerializeField] GameObject attackPrefab;
    IEnumerator AttackRoutine(Vector2 direction)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1.5f);
            for (int j = 0; j < 3; j++)
            {
                GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);

                Vector2 startPosition = projectile.transform.position;
                Vector2 targetPosition = direction;

                float elapsedTime = 0f;
                float duration = 0.8f;

                while (elapsedTime < duration)
                {
                    projectile.transform.position = Vector2.Lerp(startPosition, targetPosition + new Vector2(5,0), elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                Destroy(projectile);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1);
        }
    }

}
