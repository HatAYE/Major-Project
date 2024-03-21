using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : Enemy
{
    [SerializeField] float munchCoolDownMax;
    float timer;

    void Start()
    {
        base.Start();
        timer=munchCoolDownMax;
    }

    void Update()
    {
        base.Update();
        if (playerInRadius==true)
        {
            TransitionToState(EnemyState.attack);
        }

    }
    protected override void IdleState()
    {
        //PLAY IDLE ANIMATION
    }
    protected override void AttackingState()
    {
        if (playerInRadius==true)
        {
            timer-=0.05F;
            //PLAY MUNCHING ANIMATION
            if (timer<=0)
            {
                player.GetComponent<HealthSystem>().currentHealth--;
                timer=munchCoolDownMax;
            }
        }
        else timer=munchCoolDownMax;
    }

    protected override void DieState()
    {
        //HE CAN ONLY BE KILLED BY GETTING SQUISHED UNDER A BOX
        Destroy(gameObject);
    }

    
}
