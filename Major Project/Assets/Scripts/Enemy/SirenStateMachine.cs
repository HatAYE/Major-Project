using Conversa.Runtime;
using System.Collections;
using UnityEngine;
public class SirenStateMachine : Enemy
{
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Conversation startingConvo;
    [SerializeField] Conversation endingConvo;
    [SerializeField] Collider2D rightTrigger;
    [SerializeField] Vector2 projectileOffset;
    DialogueController dialogueController = new DialogueController();
    GameObject musicTrails;
    Animator animator;
    protected override void Start()
    {
        base.Start();
        musicTrails = transform.GetChild(1).gameObject;
        AudioManager.Instance.PlaySound(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.princessSinging);
        animator = GetComponent<Animator>();
        player.onPlayerDeath += ActivateMusicTrails;
        battleMusic= AudioManager.Instance.princessBattleMusic;
        ogMusic = AudioManager.Instance.musicSource.clip;
    }

    protected override void Update()
    {
        if (playerInRadius)
        {
            startingCoroutine = StartCoroutine(EnemyBehavior());
            playerInRadius = false;
        }
    }
    
    IEnumerator EnemyBehavior()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, battleMusic);
        yield return BeginDialogueCoroutine();
        player.gameObject.GetComponent<BlockAreas>().BlockArea();
        yield return new WaitUntil(() => currentState == EnemyState.attack);
        // attack logic
        yield return AttackRoutine();

        // dialogue ending
        yield return FinalDialogueCoroutine();
        AudioManager.Instance.PlaySound(AudioManager.Instance.musicSource, ogMusic);
        player.gameObject.GetComponent<BlockAreas>().UnlockArea();
        yield return new WaitUntil(() => currentState == EnemyState.die);
        // go home
        DieState();
    }
    protected override void IdleState()
    {
        //PLAY IDLE ANIMATION

        /*float amplitude = 0.002f;
        float speed = 1f;
        float initialY = transform.position.y;

        float newY = initialY + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);*/
        
    }
    void ActivateMusicTrails()
    {
        musicTrails.gameObject.SetActive(true);
        StartCoroutine(musicTrails.GetComponent<MusicTrail>().FadeInMusicTrails());
        StartCoroutine(AudioManager.Instance.FadeIn(gameObject.GetComponent<AudioSource>(), AudioManager.Instance.princessSinging));
    }
    IEnumerator BeginDialogueCoroutine()
    {
        areaDetector.SetActive(false);
        //fadeout music and trail
        StartCoroutine(musicTrails.GetComponent<MusicTrail>().FadeOutMusicTrails());
        musicTrails.gameObject.SetActive(false);
        StartCoroutine(AudioManager.Instance.FadeOut(gameObject.GetComponent<AudioSource>()));
        animator.SetTrigger("idle");
        if (dialogueController != null)
        {
            dialogueController.NewConversation(startingConvo);
            dialogueController.BeginDialogue();
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.attack);
            yield return new WaitForSeconds(1);
        }
    }

    bool attacked;
    protected override void AttackingState()
    {
        if (!attacked)
        {
            StartCoroutine(AttackRoutine());
            attacked = true;
        }
    }
    IEnumerator AttackRoutine()
    {
        Vector2 attackDirection = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1.5f);
            for (int j = 0; j < 3; j++)
            {
                animator.SetTrigger("attack");
                GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);
                Vector2 targetDirection = (attackDirection + projectileOffset).normalized;
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                projectileRb.velocity = targetDirection * 8;
                projectile.GetComponent<Projectile>().parentEnemy = gameObject;
                animator.SetTrigger("idle");
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1);
        }
        
        TransitionToState(EnemyState.dialogue2);
    }
    bool gaveHeart=false;
    IEnumerator FinalDialogueCoroutine()
    {
        animator.SetTrigger("idle");
        if (dialogueController != null)
        {
            dialogueController.NewConversation(endingConvo);
             dialogueController.BeginDialogue();
            //PLAY ANIMATION OF SIREN GIVING A HEART
            dialogueController.OnDialogueEnd += () => TransitionToState(EnemyState.die);
            yield return new WaitForSeconds(1);
        }
        if (!gaveHeart)
        {
            player.onEssenceCollection -= musicTrails.GetComponent<MusicTrail>().UpdateMusicTrail;
            StartCoroutine(player.AddEssence());
            gaveHeart = true;
        }
        animator.SetTrigger("poof");
        yield return new WaitForSeconds(1.5f);
    }
    //bool flipped;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            if (collision = rightTrigger)
            {
                //if(!flipped)
                //{
                    transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
                    //flipped = true;
                //}
            }
        }
    }
    protected override void DieState()
    {
        player.onPlayerDeath -= ActivateMusicTrails;
        player.onPlayerDeath -= ResetStateMachine;
        player.onEssenceCollection -= musicTrails.GetComponent<MusicTrail>().UpdateMusicTrail;
        attacked =false;
        Destroy(areaDetector);
        Destroy(gameObject);
    }
}
