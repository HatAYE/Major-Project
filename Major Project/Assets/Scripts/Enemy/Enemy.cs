using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
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
    public bool playerInRadius;
    public GameObject areaDetector;

    protected virtual void Start()
    {
        player= FindObjectOfType<Controls>();
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                IdleState();
                break;
            case EnemyState.attack:
                AttackingState();
                break;
            case EnemyState.die:
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
}
