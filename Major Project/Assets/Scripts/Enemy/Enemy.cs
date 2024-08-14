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
    protected AudioClip battleMusic;
    public AudioClip ogMusic;
    protected Animator animator;
    protected virtual void Start()
    {
        player= FindObjectOfType<Controls>();
        player.onPlayerDeath += ResetStateMachine;
        currentState= EnemyState.idle;
        ogMusic = AudioManager.Instance.musicSource.clip;
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
                player.onPlayerDeath -= ResetStateMachine;
                DieState();
                break;
        }
    }

    protected abstract void IdleState();
    protected abstract void AttackingState();
    protected abstract void DieState();
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
        AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, ogMusic);
    }
    protected void ResetStateMachine()
    {
        if(startingCoroutine!=null)
        StopAllCoroutines();
        areaDetector.SetActive(true);
        currentState =EnemyState.idle;
        if (animator != null)
        {
            animator.SetTrigger("idle");
        }
        AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, ogMusic);
    }
}
