using Conversa.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Conversation conversation;
    [SerializeField] float attackPause;
    [HideInInspector] public bool gotHit;
    DialogueController controller;
    bool startDialogue;
    protected override void Start()
    {
        base.Start();
        controller = new DialogueController(conversation);
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == EnemyState.dialogue1)
            DialogueState();

        if (playerInRadius && !startDialogue)
        {
            TransitionToState(EnemyState.dialogue1);
            startDialogue = true;
        }

        if (gotHit)
        {
            currentState=EnemyState.die;
        }
    }
    protected override void IdleState()
    {
        float amplitude = 0.002f;
        float speed = 1f;
        float initialY = transform.position.y;

        float newY = initialY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    void DialogueState()
    {
        startDialogue = false;
        StartCoroutine(Dialogue());
    }
    IEnumerator Dialogue()
    {
        if (controller != null)
        {
            controller.BeginDialogue();
            controller.OnDialogueEnd += () => currentState = EnemyState.attack;
            yield return new WaitForSeconds(1);
        }
    }
    bool isAttacking;
    float attackTimer = 10;
    protected override void AttackingState()
    {
        if (!isAttacking)
        {
            attackTimer -= 0.05f;
            if (attackTimer <= 0)
            {
                StartCoroutine(Attack());
                attackTimer = 0;
            }
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
        yield return new WaitForSeconds(attackPause);
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
    protected override void DieState()
    {
        //PLAY ANIMATION
        Destroy(areaDetector);
        Destroy(gameObject);
    }
}
