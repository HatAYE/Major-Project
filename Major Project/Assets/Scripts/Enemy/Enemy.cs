using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour , IResettable
{
    public enum EnemyState
    {
        idle,
        attack,
        die,
        dialogue1,
        dialogue2,
    }
    protected Controls player;
    protected EnemyState currentState=EnemyState.idle;
    protected Coroutine startingCoroutine;
    [HideInInspector] public bool playerInRadius;
    [HideInInspector] public GameObject areaDetector;

    protected virtual void Start()
    {
        player= FindObjectOfType<Controls>();
        player.onPlayerDeath += ResetStateMachine;
        currentState= EnemyState.idle;
        //player.inCombat = false;
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                //player.inCombat = false;
                IdleState();
                break;
            case EnemyState.attack:
                //player.inCombat= true;
                AttackingState();
                break;
            case EnemyState.die:
                //player.inCombat = false;
                DieState();
                break;
        }
    }

    protected abstract void IdleState();
    protected abstract void AttackingState();
    protected abstract void DieState();
   // protected abstract IEnumerator EnemyBehavior();
    protected void TransitionToState(EnemyState newState)
    {
        currentState = newState;
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        currentState=EnemyState.idle;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Idle");
        }
    }
    void ResetStateMachine()
    {
        StopAllCoroutines();
        areaDetector.SetActive(true);
        currentState =EnemyState.idle;
        print("current coroutine is " + startingCoroutine);
    }
}
