using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    RaycastHit2D hit;
    Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] KeyCode left=KeyCode.A;
    [SerializeField] KeyCode right=KeyCode.D;
    public KeyCode jump;
    [SerializeField] KeyCode pushAndPull;
    [SerializeField] KeyCode crouch;

    #region movement variables
    [SerializeField] float playerSpeed;
    [HideInInspector] public bool movingRight;
    [HideInInspector] public bool isMoving;
    #endregion

    #region jumping variables
    [SerializeField] float jumpForce;
    int jumpCount;
    #endregion

    #region push and pull variables
    [HideInInspector] public bool isHoldingObject;
    bool letGoOfObject;
    bool justPushed;
    GameObject holdObject;
    float pushCoolDown;
    #endregion

    #region crouch variables
    [HideInInspector] public bool isCrouching;
    float crouchSpeed;
    Vector2 crouchHeight;
    Vector2 colOffset;
    Vector2 normalHeight;
    Vector2 normalColOffset;
    //RaycastHit2D crouchRay;
    [SerializeField] LayerMask aboveObject;
    #endregion

    #region sprinting variables
    float sprintingSpeed;
    #endregion

    #region Essence variables
    public int essenceCollected;
    [SerializeField] Text essenceText;
    public Action onEssenceCollection;
    #endregion

    #region Swinging variables
    HingeJoint2D hj;
    bool isAttached;
    Transform attachedTo;
    GameObject disregard;
    [SerializeField] float swingingForce;
    RopeSegment ropeSegment;
    #endregion

    #region Respawning
    [SerializeField] Transform respawnPoint;
    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hj= GetComponent<HingeJoint2D>();

        boxCollider= GetComponent<BoxCollider2D>();
        normalHeight = boxCollider.size;
        //crouchHeight = new Vector2(boxCollider.size.x, boxCollider.size.y / 2f);
        crouchHeight = new Vector2(boxCollider.size.x, 1.18f);
        colOffset = new Vector2(boxCollider.offset.x, -0.1262648f);

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
        Swinging();
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
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            Respawn();
        }
        if (!isAttached) 
        {
            if (collision.gameObject.name.StartsWith("RopeSegment"))
            {
                if (attachedTo!=collision.gameObject.transform.parent)
                {
                    if (disregard==null || collision.gameObject.transform.parent.gameObject!=disregard)
                    {
                        Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }
        
    }
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("RopeSegment"))
        {
            attachedTo = null;
        }
        
    }*/
    void ResetPlayerBools()
    {
        isMoving=false;
        isCrouching=false;
        isHoldingObject = false;
        letGoOfObject = false;
        justPushed = false;
    }
    public IEnumerator AddEssence()
    {
        essenceCollected++;
        onEssenceCollection?.Invoke();
        yield return null;
    }
    public void Respawn()
    {
        ResetPlayerBools();
        transform.position = respawnPoint.position;
    }

    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
    void Movement()
    {
        if (Input.GetKey(left) && !isAttached && !letGoOfObject)
        {
            movingRight = false;
            isMoving = true;
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
        else if (Input.GetKeyUp(right))
            isMoving = false;
        if (Input.GetKey(KeyCode.D) && !isAttached && !letGoOfObject)
        {
            movingRight=true;
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
        else if (Input.GetKeyUp(KeyCode.D))
            isMoving = false;
    }

    void Jumping()
    {
        if (Input.GetKeyDown(jump) && jumpCount == 0 && !isCrouching)
        {
            rb.AddForce(new Vector2 (0, jumpForce));
            jumpCount++;
        }
    }
    void PushingAndPulling()
    {
        if (Input.GetKey(pushAndPull))
        {
            if (!justPushed)
            {
                if (!isHoldingObject && hit.collider != null && hit.collider.TryGetComponent(out Pushable pushable))
                {
                    isHoldingObject = true;
                    holdObject = pushable.gameObject;
                    pushable.beingHeld = true;
                    pushable.rb.mass = pushable.massWhenHeld;
                    pushable.GetComponent<FixedJoint2D>().enabled = true;
                    pushable.GetComponent<FixedJoint2D>().connectedBody = rb;
                    justPushed = true;
                }
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
            
            if(justPushed)
            {
                letGoOfObject = true;
                isMoving = false;
                if (pushCoolDown <= 1.2f)
                {
                    pushCoolDown+=1 *Time.deltaTime;
                    //unavailable pushing/pulling UI acivated 
                }
                else
                {
                    justPushed= false;
                    letGoOfObject= false;
                    pushCoolDown = 0;
                    //available pushing/pulling UI acivated
                }
            }
            
        }
    }
    void Crouch()
    {
        //crouchRay= Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, 1.5f, aboveObject);
        Collider2D crouchCollider = Physics2D.OverlapBox(transform.position + new Vector3(0, 1, 0), new Vector2(boxCollider.size.x, 1), 1f, aboveObject);
        if (Input.GetKeyDown(crouch) || Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching)
            {
                boxCollider.size = crouchHeight;
                boxCollider.offset = colOffset;
                //spriteRenderer.sprite = standingAndCrouchingSprites[1];
                isCrouching = true;
            }
            else
            {
                if (crouchCollider == null)
                {
                    boxCollider.size = normalHeight;
                    boxCollider.offset = normalColOffset;
                    //spriteRenderer.sprite = standingAndCrouchingSprites[0];
                    isCrouching = false;
                }
                else if (crouchCollider.gameObject.layer == aboveObject) return;
            }
        }
    }

    void DeflectingShield()
    {
        //SHIELD WILL BE ACTIVATED ONLY AT CHAPTER 3
        if (transform.GetChild(1).gameObject!=null)
        {
            if (Input.GetKey(KeyCode.J))
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    #region Swinging
    void Swinging()
    {
        /*if (Input.GetKey(KeyCode.W) && isAttached)
        {
            Slide(1);
        }*/
        if (Input.GetKey(KeyCode.D) && isAttached)
        {
            rb.AddRelativeForce(new Vector2(1, 0) * swingingForce);
        }
        if (Input.GetKey(KeyCode.A) && isAttached)
        {
            rb.AddRelativeForce(new Vector2(-1, 0) * swingingForce);
        }
        if (Input.GetKey(KeyCode.S) && isAttached)
        {
            Slide(-1);
        }
        if (Input.GetKeyDown(KeyCode.W) && isAttached)
        {
            StartCoroutine(Detach());
            rb.AddForce(new Vector2(0, jumpForce));
            jumpCount++;
        }
        StartCoroutine(AutomaticSlide());
            
    }
    IEnumerator AutomaticSlide()
    {
        if (ropeSegment != null)
        {
            if (ropeSegment.connectedBelow != null)
            {
                Slide(-1);
                yield return new WaitForSeconds(5f);
                //the delay still doesnt work :(
            }
        }
    }
    void Attach(Rigidbody2D ropeBone)
    {
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached=true;
        hj.connectedBody = ropeBone;
        hj.enabled= true;
        isAttached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
        ropeSegment = ropeBone.gameObject.GetComponent<RopeSegment>();
    }

    IEnumerator Detach()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
        isAttached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        yield return new WaitForSeconds(0.5f);
        attachedTo = null;
        ropeSegment= null;  
    }
    public void Slide(int direction)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        if(direction>0)
        {
            if (myConnection.connectedAbove!=null)
            {
                if (myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    newSeg = myConnection.connectedAbove;
                }
            }
        }
        else
        {
            if(myConnection.connectedBelow!=null)
            {
                newSeg=myConnection.connectedBelow;
            }
        }
        if (newSeg!=null)
        {
            transform.position=newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            hj.connectedBody=newSeg.GetComponent<Rigidbody2D>();
        }
    }
    #endregion
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
