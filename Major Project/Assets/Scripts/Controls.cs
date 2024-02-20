using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    RaycastHit2D hit;
    Rigidbody2D rb;
    bool isMoving = false;
    bool movingRight;
    bool movingLeft;
    private BoxCollider2D boxCollider;

    #region jumping variables
    [SerializeField] float jumpForce;
    [SerializeField] float fallMultiplier;
    int jumpCount;
    #endregion
    #region push and pull variables
    [SerializeField] bool isHoldingObject;
    Vector2 holdOffset; 
    public float grabbingDistance;
    private GameObject holdObject;
    //Rigidbody2D moveableRb;
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
        if (movingLeft) hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, grabbingDistance, LayerMask.GetMask("Obstacle"));
        else if (movingRight) hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, grabbingDistance, LayerMask.GetMask("Obstacle"));
        Movement();
        Jumping();
        PushingAndPulling();
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
            movingRight = false;
            movingLeft = true;
            if (!isCrouching)
            {
                rb.velocity = new Vector2(-playerSpeed, rb.velocity.y);
                Debug.Log("tryna move left");
            }
            isMoving = true;
            if (isCrouching) rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);
            
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movingRight=true;
            movingLeft = false;
            if (!isCrouching) rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
            isMoving = true;
            if (isCrouching) rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);
            hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 1.5f, LayerMask.GetMask("Obstacle"));
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

        if (movingRight)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * grabbingDistance);
        }
        else if (movingLeft)
        {
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * grabbingDistance);
        }
    }
}
