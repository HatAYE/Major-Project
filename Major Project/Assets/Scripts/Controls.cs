using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] int playerSpeed;
    Rigidbody2D rb;
    bool isMoving = false;
    private BoxCollider2D boxCollider;

    #region jumping variables
    [SerializeField] int jumpForce;
    [SerializeField] int fallMultiplier;
    int jumpCount;
    #endregion
    #region push and pull variables
    bool isHoldingObject;
    Vector2 holdOffset; 
    public float pullForce = 30f;
    private GameObject holdObject;
    Rigidbody2D moveableRb;
    #endregion
    #region crouch variables
    public float crouchSpeed;
    public float crouchColliderHeight;
    bool isCrouching;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Movement();
        Jumping();
        PushingAndPulling();
        //Crouch();
        if (isHoldingObject && holdObject != null) // Check if we're holding an object and it's not null
        {
            Vector2 targetPosition = (Vector2)transform.position + holdOffset;
            holdObject.transform.position = Vector2.MoveTowards(holdObject.transform.position, targetPosition, pullForce * Time.fixedDeltaTime);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.CompareTag("Moveable"))
        {
            jumpCount = 0;
        }
        if (collision.gameObject.layer== LayerMask.NameToLayer("Obstacle") && !isHoldingObject)
        {
            rb.velocity= Vector2.zero;
        }
    }
    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!isCrouching) rb.velocity = new Vector2(-playerSpeed, rb.velocity.y);
            isMoving = true;
            if (isCrouching) rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isCrouching) rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
            isMoving = true;
            if (isCrouching) rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);

        }
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0 && !isHoldingObject)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
        if (rb.velocity.y < 0 )
        {
            rb.velocity = new Vector2(rb.velocity.x, -fallMultiplier);
        }
    }

    void PushingAndPulling()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHoldingObject)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Moveable"))
                    {
                        holdObject = collider.gameObject;
                        isHoldingObject = true;
                        holdOffset = (Vector2)holdObject.transform.position - (Vector2)transform.position;
                        moveableRb = holdObject.GetComponent<Rigidbody2D>();
                        if (moveableRb != null)
                        {
                            moveableRb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                            moveableRb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                        }
                        break;
                    }
                }
            }
            else
            {
                moveableRb.constraints = RigidbodyConstraints2D.FreezeAll;
                holdObject = null;
                isHoldingObject = false;
            }
        }
    }
    
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            boxCollider.size = new Vector2(boxCollider.size.x, crouchColliderHeight);
        }
        else
        {
            boxCollider.size = new Vector2(boxCollider.size.x, 1f);  
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D collider in colliders)
        {
            Gizmos.DrawWireSphere(collider.transform.position, 2f);
        }

    }
}
