using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    bool movingRight;
    
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
    //RaycastHit2D crouchRay;
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
        center = transform.position;
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite= standingAndCrouchingSprites[0];

        boxCollider= GetComponent<BoxCollider2D>();
        normalHeight = boxCollider.size;
        crouchHeight = new Vector2(boxCollider.size.x, boxCollider.size.y / 2f);

        crouchSpeed = playerSpeed / 2;
        sprintingSpeed = playerSpeed * 2;
    }
    void Update()
    {
        if (movingRight==false) hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, 1f, LayerMask.GetMask("Pushable"));
        else hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 1f, LayerMask.GetMask("Pushable"));


        if (essenceText != null)
        essenceText.text = "Essence collected: " + essenceCollected.ToString();

        Movement();
        Jumping();
        PushingAndPulling();
        Crouch();
        DeflectingShield();

        collectedSparepart = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
    bool collectedSparepart;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Spare part") && !collectedSparepart)
        {
            Destroy(collision.gameObject);
            LevelOneManager.scrapParts ++;
            collectedSparepart=true;
        }
    }
    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movingRight = false;

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
        if (Input.GetKeyDown(KeyCode.W) && jumpCount == 0 && !isCrouching)
        {
            rb.AddForce(new Vector2 (0, jumpForce));
            jumpCount++;
        }
    }
    void PushingAndPulling()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            
            if (!isHoldingObject && hit.collider!=null && hit.collider.TryGetComponent(out Pushable pushable))
            {
                isHoldingObject = true;
                holdObject= pushable.gameObject;
                pushable.beingHeld = true;
                pushable.rb.mass = pushable.massWhenHeld;
                pushable.GetComponent<FixedJoint2D>().enabled = true;
                pushable.GetComponent<FixedJoint2D>().connectedBody=rb;
            }
            
        }
        else
        {
            if (holdObject != null)
            {
                holdObject.GetComponent<FixedJoint2D>().enabled = false;
                holdObject.GetComponent<Pushable>().beingHeld = false;
            }
            holdObject = null;
            isHoldingObject = false;
        }
    }
    [SerializeField] Vector2 center;
    void Crouch()
    {
        //crouchRay= Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, 1.5f, aboveObject);
        Collider2D crouchCollider = Physics2D.OverlapBox(transform.position + new Vector3(0, 1, 0), new Vector2(boxCollider.size.x, 1), 1f, aboveObject);
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
                if (crouchCollider == null)
                {
                    boxCollider.size = normalHeight;
                    spriteRenderer.sprite = standingAndCrouchingSprites[0];
                    isCrouching = false;
                }
                else if (crouchCollider.gameObject.layer == aboveObject) return;
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
        Gizmos.color = Color.magenta;

        if (movingRight)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * 1f);
        }
        else
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * 1f);
        }
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x* 1.5f);
        if (boxCollider!=null)
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), new Vector3(boxCollider.size.x, 1f, 0f)) ;
    }
}
