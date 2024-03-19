using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Dialogue,
        Attacking,
        Die
    }
    GameObject player;
    [SerializeField] EnemyState currentState;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] DialogueController dialogueController;
    void Start()
    {
        currentState = EnemyState.Idle;
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
            case EnemyState.Die:
                DieState();
                break;

            default:
                break;
        }

        if (gotHit)
        {
            currentState=EnemyState.Die;
        }
    }

    void IdleState()
    {
        float amplitude = 0.002f;
        float speed = 1f;
        float initialY = transform.position.y;

        float newY = initialY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void DialogueState()
    {
        StartCoroutine(Dialogue());
    }
    IEnumerator Dialogue()
    {
        if (dialogueController != null)
        {
            dialogueController.BeginDialogue();
            dialogueController.OnDialogueEnd += () => currentState = EnemyState.Attacking;
            yield return new WaitForSeconds(1);
        }
    }
    bool isAttacking;
    void AttackingState()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        Vector2 attackDirection = player.transform.position;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 targetDirection = ((Vector3)attackDirection - projectile.transform.position).normalized;

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = targetDirection * 8;
        projectile.GetComponent<Projectile>().parentEnemy = gameObject;


        float angleOffset = Mathf.PI / 6;
        Vector2 leftTargetDirection = RotateVector2(targetDirection, angleOffset);
        Vector2 rightTargetDirection = RotateVector2(targetDirection, -angleOffset);

        GameObject leftProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        leftProjectile.GetComponent<Rigidbody2D>().velocity = leftTargetDirection * 8;
        leftProjectile.GetComponent<Projectile>().parentEnemy = gameObject;

        GameObject rightProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        rightProjectile.GetComponent<Rigidbody2D>().velocity = rightTargetDirection * 8;
        rightProjectile.GetComponent<Projectile>().parentEnemy = gameObject;
        isAttacking = true;
        yield return new WaitForSeconds(3);
        isAttacking = false;

    }
    Vector2 RotateVector2(Vector2 vector, float angle)
    {
        float cosAngle = Mathf.Cos(angle);
        float sinAngle = Mathf.Sin(angle);
        return new Vector2(
            vector.x * cosAngle - vector.y * sinAngle,
            vector.x * sinAngle + vector.y * cosAngle
        );
    }
    public bool gotHit;
    void DieState()
    {
        //PLAY ANIMATION
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            currentState = EnemyState.Dialogue;
        }
    }
}
