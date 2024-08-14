using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : Enemy
{
    [SerializeField] float jumpForce;
    [SerializeField] float jumpPause;
    [SerializeField] bool damaged;
    Rigidbody2D rb;
    [SerializeField] bool attacking;
    Collider2D triggerCol;
    Collider2D nonTriggerCol;
    Animator animator;
    protected override void Start()
    {
        base.Start();
        rb=GetComponent<Rigidbody2D>();
        ogMusic = AudioManager.Instance.musicSource.clip;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                nonTriggerCol = collider;
                break;
            }
            else triggerCol = collider;
        }
        triggerCol.enabled = false;

        animator=transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void Update()
    {
        Physics2D.IgnoreCollision(nonTriggerCol, player.GetComponent<Collider2D>(), true);
        if (playerInRadius)
        {
            StartCoroutine(Attack());
        }
        else currentState = EnemyState.idle;

        animator.SetBool("attacking", attacking);
    }
    protected override void AttackingState()
    {

    }
    IEnumerator Attack()
    {
        if (!attacking)
        {
            attacking = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            triggerCol.enabled = false;
            //play animation. animator state machine will play jump first then when jump is done itll transition to radiation animation
            yield return new WaitForSeconds(jumpPause);
            attacking = false;
        }
    }
    protected override void DieState()
    {

    }

    protected override void IdleState()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            if(!damaged)
            {
                player.GetComponent<HealthSystem>().Damage(1);
                damaged = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            damaged = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            triggerCol.enabled = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pushable"))
        {
            Collider2D otherCollider = collision.collider;

            // Ignore collision between this object and the pushable object
            Physics2D.IgnoreCollision(nonTriggerCol, otherCollider, true);
        }
    }
    private void OnDrawGizmos()
    {
        if (triggerCol != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(triggerCol.bounds.center, triggerCol.bounds.size);
        }
    }
}
