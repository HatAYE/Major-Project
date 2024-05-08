using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] int level { get; set; }
    Controls player;
    Animator animator;
    [SerializeField] AnimationClip clip;
    //CHANGE ANIMATOR CONTROLLER DEPENDING ON ESSENCE COLLECTED
    void Start()
    {
        player = FindObjectOfType<Controls>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z) *1 ;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("jumped");
        }

        animator.SetBool("canMove", player.isMoving);
        animator.SetBool("canCrouch", player.isCrouching);
        animator.SetBool("canPushAndPull", player.isHoldingObject);
    }

}
