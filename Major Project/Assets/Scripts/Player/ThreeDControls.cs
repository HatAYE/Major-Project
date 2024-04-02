using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDControls : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxCollider;
    SpriteRenderer spriteRenderer;
    RaycastHit hit;

    bool movingRight;
    float playerSpeed= 6;
    float sprintingSpeed;

    float jumpForce = 1300;
    int jumpCount;

    bool isHoldingObject;
    private GameObject holdObject;

    bool isCrouching;
    [SerializeField] Sprite[] standingAndCrouchingSprites = new Sprite[2];
    float crouchSpeed;
    Vector3 crouchHeight;
    Vector3 normalHeight;
    RaycastHit crouchRay;
    [SerializeField] LayerMask aboveObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = standingAndCrouchingSprites[0];

        boxCollider = GetComponent<BoxCollider>();
        normalHeight = boxCollider.size;
        crouchHeight = boxCollider.size / 2;

        crouchSpeed = playerSpeed / 2;
        sprintingSpeed = playerSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 raycastDirection = movingRight ? transform.right : -transform.right;
        if (Physics.Raycast(transform.position, raycastDirection * transform.localScale.x, out hit, 1f, LayerMask.GetMask("Obstacle")))
        {
            PushingAndPulling();
        }


        Movement();
        Jumping();
        
        Crouch();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movingRight = false;

            if (!isCrouching)
            {
                if (Input.GetKey(KeyCode.LeftShift))
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
            movingRight = true;

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
            rb.AddForce(new Vector3(0, jumpForce));
            jumpCount++;
        }
    }
    void PushingAndPulling()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isHoldingObject && hit.collider != null && hit.collider.gameObject.tag == "Moveable")
            {
                isHoldingObject = true;
                holdObject = hit.collider.gameObject;
                holdObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX;
                holdObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
                holdObject.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
            }

        }
        else
        {
            if (holdObject != null)
            {
                holdObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                holdObject.GetComponent<FixedJoint>().connectedBody = null;
            }
            holdObject = null;
            isHoldingObject = false;
        }
    }

    void Crouch()
    {
        if (Physics.Raycast(transform.position, Vector2.up * transform.localScale.x * transform.localScale.x, out crouchRay, 1f, aboveObject))
        {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Moveable"))
        {
            jumpCount = 0;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (movingRight)
        {
            Gizmos.DrawLine(transform.position,transform.position + Vector3.right * transform.localScale.x * 1f);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * transform.localScale.x * 1f);
        }
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * transform.localScale.x * 1.5f);
    }
}
