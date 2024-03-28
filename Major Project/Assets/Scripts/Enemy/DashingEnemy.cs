using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : Enemy
{
    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;
    [SerializeField] float speed;
    bool dashRight;
    
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerInRadius)
        {
            StartCoroutine(Attack());
        }
    }
    protected override void IdleState()
    {
        //play animation
    }
    protected override void AttackingState()
    {
    }
    IEnumerator Attack()
    {
        if (!dashRight)
        {
            if (Vector3.Distance(transform.position, pos1.transform.position) >= 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, pos1.transform.position, speed);
            }
            else
            {
                yield return new WaitForSeconds(2);
                dashRight = true;
            }
        }
        if (dashRight)
        {
            if (Vector2.Distance(transform.position, pos2.transform.position) >= 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, pos2.transform.position, speed);
            }
            else
            {
                yield return new WaitForSeconds(2);
                dashRight = false;
            }
        }
    }
    protected override void DieState()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==player.gameObject)
        {
            player.GetComponent<HealthSystem>().currentHealth -= 1;
        }
    }

}
