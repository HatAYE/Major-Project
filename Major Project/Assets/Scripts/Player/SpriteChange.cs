using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] int level { get; set; }
    Controls player;
    Animator animator;
    //CHANGE ANIMATOR CONTROLLER DEPENDING ON ESSENCE COLLECTED
    void Start()
    {
        player = FindObjectOfType<Controls>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        animator.SetBool("canMove", player.isMoving);
    }
}
