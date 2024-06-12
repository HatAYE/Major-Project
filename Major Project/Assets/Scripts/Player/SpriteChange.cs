using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    Controls player;
    Animator animator;
    [SerializeField] AnimationClip clip;
    bool facingRight;
    void Start()
    {
        player = FindObjectOfType<Controls>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!player.isHoldingObject)
        {
            if (!player.movingRight && !facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = true;
            }
            else if (player.movingRight && facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = false;
            }
        }

        if (Input.GetKeyDown(player.jump))
        {
            animator.SetTrigger("jumped");
        }

        animator.SetBool("canMove", player.isMoving);
        animator.SetBool("canCrouch", player.isCrouching);
        animator.SetBool("canPushAndPull", player.isHoldingObject);
    }

}
