using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : Enemy
{
    [SerializeField] GameObject pos1;
    [SerializeField] GameObject pos2;
    [SerializeField] float speed;
    bool dashRight;
    bool playedAudio;
    protected override void Start()
    {
        base.Start();
        ogMusic = AudioManager.Instance.musicSource.clip;
        animator= transform.GetChild(0).GetComponent<Animator>();
        player.onPlayerDeath += resetBools;
        battleMusic = AudioManager.Instance.enemyBattleMusic;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerInRadius)
        {
            playedAudio = false;
            StartCoroutine(Attack());
            if (!intiatedAttack)
            {
                animator.SetTrigger("attack");
                intiatedAttack = true;
                StartCoroutine(AudioManager.Instance.FadeIn(AudioManager.Instance.musicSource, battleMusic));
                //AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, battleMusic);
            }
        }
        if(!playerInRadius && !playedAudio)
        {
            StartCoroutine(AudioManager.Instance.FadeIn(AudioManager.Instance.musicSource, ogMusic));
            //AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, ogMusic);
            intiatedAttack = false;
            playedAudio = true;
        }
    }
    protected override void IdleState()
    {
        //play animation
    }
    protected override void AttackingState()
    {
    }
    [SerializeField] bool intiatedAttack;
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
    void resetBools()
    {
        damaged = false;
        intiatedAttack = false;
       //animator.SetTrigger("idle");
    }
    protected override void DieState()
    {

    }
    bool damaged;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthSystem player))
        {
            if(!damaged)
            {
                player.Damage(1);
                damaged = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthSystem player))
        {
            damaged = false;
        }

    }

}
