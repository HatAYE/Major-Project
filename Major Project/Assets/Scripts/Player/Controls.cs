using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    RaycastHit2D hit;
    Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    #region movement variables
    [SerializeField] float playerSpeed;
    bool isMoving = false;
    bool movingRight;
    bool movingLeft;
    #endregion

    #region jumping variables
    [SerializeField] float jumpForce;
    int jumpCount;
    #endregion

    #region push and pull variables
    bool isHoldingObject;
    private GameObject holdObject;
    #endregion

    #region crouch variables
    [SerializeField] bool isCrouching;
    [SerializeField] Sprite[] standingAndCrouchingSprites = new Sprite[2];
    float crouchSpeed;
    Vector2 crouchHeight;
    Vector2 normalHeight;
    RaycastHit2D crouchRay;
    [SerializeField] LayerMask aboveObject;
    #endregion

    #region sprinting variables
    float sprintingSpeed;
    #endregion

    #region Essence variables
    public int essenceCollected;
    [SerializeField] Text essenceText;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite= standingAndCrouchingSprites[0];

        boxCollider= GetComponent<BoxCollider2D>();
        normalHeight = boxCollider.size;
        crouchHeight = boxCollider.size / 2;

        crouchSpeed = playerSpeed / 2;
        sprintingSpeed = playerSpeed * 2;
    }
    void Update()
    {
        if (movingLeft) hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, 1f, LayerMask.GetMask("Obstacle"));
        else if (movingRight) hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 1f, LayerMask.GetMask("Obstacle"));
        
        if (essenceText != null)
        essenceText.text = "Essence collected: " + essenceCollected.ToString();

        Movement();
        Jumping();
        PushingAndPulling();
        Crouch();
        DeflectingShield();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Moveable"))
        {
            jumpCount = 0;
        }
    }
    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movingRight = false;
            movingLeft = true;
            isMoving = true;

            if (!isCrouching)
            {
                if (Input.GetKey (KeyCode.LeftShift))
                {
                    rb.velocity = new Vector2(-sprintingSpeed, rb.velocity.y);
                }
                else rb.velocity = new Vector2(-playerSpeed, rb.velocity.y);
            }

            if (isCrouching) 
                rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movingRight=true;
            movingLeft = false;
            isMoving = true;

            if (!isCrouching)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rb.velocity = new Vector2(sprintingSpeed, rb.velocity.y);
                }
                else rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
            }

            if (isCrouching)
                rb.velocity = new Vector2(crouchSpeed, rb.velocity.y);
        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && !isHoldingObject)
        {
            rb.AddForce(new Vector2 (0, jumpForce));
            jumpCount++;
        }
    }
    void PushingAndPulling()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isHoldingObject && hit.collider!=null && hit.collider.gameObject.tag== "Moveable")
            {
                isHoldingObject = true;
                holdObject=hit.collider.gameObject;
                holdObject.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                holdObject.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                holdObject.GetComponent<FixedJoint2D>().enabled = true;
                holdObject.GetComponent<FixedJoint2D>().connectedBody=this.GetComponent<Rigidbody2D>();
            }
            
        }
        else
        {
            if (holdObject != null)
            {
                holdObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                holdObject.GetComponent<FixedJoint2D>().enabled = false;
            }
            holdObject = null;
            isHoldingObject = false;
        }
    }
    
    void Crouch()
    {
        crouchRay= Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, 1.5f, aboveObject);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                boxCollider.size = crouchHeight;
                spriteRenderer.sprite = standingAndCrouchingSprites[1];
                isCrouching = true;
            }
            else
            {
                if (crouchRay.collider == null)
                {
                    boxCollider.size = normalHeight;
                    spriteRenderer.sprite = standingAndCrouchingSprites[0];
                    isCrouching = false;
                }
                else if (crouchRay.collider.gameObject.layer != aboveObject) return;
            }
        }
    }

    void DeflectingShield()
    {
        //SHIELD WILL BE ACTIVATED ONLY AT CHAPTER 3
        if (Input.GetKey(KeyCode.J))
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else transform.GetChild(1).gameObject.SetActive(false);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (movingRight)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * 1f);
        }
        else if (movingLeft)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * 1f);
        }
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x* 1.5f);
    }
}
