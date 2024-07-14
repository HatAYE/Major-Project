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
    [HideInInspector] public bool controlsAvaialble;
    private BoxCollider2D boxCollider;
    [SerializeField] KeyCode left=KeyCode.A;
    [SerializeField] KeyCode right=KeyCode.D;
    public KeyCode jump;
    [SerializeField] KeyCode pushAndPull;
    [SerializeField] KeyCode crouch;
    [HideInInspector] public bool inCombat=false;
    #region movement variables
    [SerializeField] float playerSpeed;
    [HideInInspector] public bool movingRight;
    [HideInInspector] public bool isMoving;
    #endregion

    #region jumping variables
    [SerializeField] float jumpForce;
    [HideInInspector] public bool canjump;
     public bool isfalling;
     public bool isGrounded=true;
    int jumpCount;
    #endregion

    #region push and pull variables
    [HideInInspector] public bool isHoldingObject;
    bool letGoOfObject;
    GameObject holdObject;
    [HideInInspector] public float originalPushingRange=.4f;
    public float currentPushingRange;
    public float enlargedPushingRange = 1;
    #endregion

    #region crouch variables
    [HideInInspector] public bool isCrouching;
    float crouchSpeed=4;
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
    PauseMenu pauseMenu;
    #endregion
    void Start()
    {
        essenceText = GameObject.Find("no. of essence").GetComponent<Text>();
        pauseMenu= FindObjectOfType<PauseMenu>();
        rb = GetComponent<Rigidbody2D>();
        hj= GetComponent<HingeJoint2D>();
        controlsAvaialble = true;
        boxCollider = GetComponent<BoxCollider2D>();
        normalHeight = boxCollider.size;
        crouchHeight = new Vector2(boxCollider.size.x, 1.18f);
        colOffset = new Vector2(boxCollider.offset.x, -0.1262648f);

        //crouchSpeed = playerSpeed / 2;
        sprintingSpeed = playerSpeed * 2;

        currentPushingRange = originalPushingRange;

        isGrounded = true;
    }
    [SerializeField] float offset;
    void Update()
    {
        if (movingRight==false) hit = Physics2D.CircleCast((Vector2)transform.position + Vector2.left * offset, currentPushingRange, Vector2.left, LayerMask.GetMask("Pushable"));
        else hit = Physics2D.CircleCast((Vector2)transform.position + Vector2.right * offset, currentPushingRange, Vector2.right, LayerMask.GetMask("Pushable"));
        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }

        if (essenceText != null)
        essenceText.text = essenceCollected.ToString();
        if (controlsAvaialble)
        {
            Movement();
            Jumping();
            PushingAndPulling();
            Crouch();
            DeflectingShield();
            Swinging();
            canjump = true;
        }
        else ResetPlayerBools();
        if (Input.GetKeyDown(KeyCode.O))
        {
            essenceCollected = 4;
        }
        collectedSparepart = false;
        if (Input.GetKeyDown(KeyCode.V)) transform.position = new Vector3(380, -6, 0);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isfalling = false;

            if(jumpCount > 0)
            jumpCount = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb.velocity.y < 0)
            {
                isGrounded = false;
                isfalling = true;
            }
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
        canjump = false;
        isfalling = false;
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
        foreach(Pushable obj in respawnPoint.gameObject.GetComponent<RespawnPoint>().pushableObjects)
        {
            obj.transform.position = obj.originalPosition;
        }
        if (pauseMenu.pauseMenuObj.activeSelf)
        {
            pauseMenu.TogglePause();
        }
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
                else 
                rb.velocity = new Vector2(-playerSpeed, rb.velocity.y); 
            } 

            if (isCrouching)
                rb.velocity = new Vector2(-crouchSpeed, rb.velocity.y);

        }
        else if (Input.GetKeyUp(left))
            isMoving = false;
        if (Input.GetKey(right) && !isAttached && !letGoOfObject)
        {
            movingRight=true;
            isMoving = true;
            if (!isCrouching)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rb.velocity = new Vector2(sprintingSpeed, rb.velocity.y);
                }
                else 
                rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
                
            }

            if (isCrouching)
                rb.velocity = new Vector2(crouchSpeed, rb.velocity.y);
        }
        else if (Input.GetKeyUp(right))
            isMoving = false;
    }

    void Jumping()
    {
        if (Input.GetKeyDown(jump) && jumpCount == 0 && !isCrouching)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpForce));
            jumpCount++;
        }

        if (!isGrounded)
        {
            isfalling = true;
        }
        if(isGrounded) isfalling=false;
    }
    void PushingAndPulling()
    {
        if (Input.GetKey(pushAndPull))
        {
            if (!isHoldingObject && hit.collider != null && hit.collider.TryGetComponent(out Pushable pushable))
            {
                print("holding");
                if(!pushable.conditionalOnSize)
                {
                    isHoldingObject = true;
                    holdObject = pushable.gameObject;
                    pushable.beingHeld = true;
                    pushable.rb.mass = pushable.massWhenHeld;
                    pushable.GetComponent<FixedJoint2D>().enabled = true;
                    pushable.GetComponent<FixedJoint2D>().connectedBody = rb;
                }
                else
                {
                    isHoldingObject = true;
                    holdObject = pushable.gameObject;
                    
                    if (GetComponent<ShrinkingAndEnlarging>().currentSize == GetComponent<ShrinkingAndEnlarging>().largeSize)
                    {
                        pushable.beingHeld = true;
                        pushable.rb.mass = pushable.massWhenHeld;
                        pushable.GetComponent<FixedJoint2D>().enabled = true;
                        pushable.GetComponent<FixedJoint2D>().connectedBody = rb;
                    }
                }
               
            }
        }
        else
        {
            if (holdObject != null)
            {
                holdObject.GetComponent<FixedJoint2D>().enabled = false;
                holdObject.GetComponent<Pushable>().beingHeld = false;
                holdObject.GetComponent<Pushable>().gotLetGo = true;
            }
            holdObject = null;
            isHoldingObject = false;
        }
    }
    void Crouch()
    {
        //crouchRay= Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, 1.5f, aboveObject);
        Collider2D crouchCollider = Physics2D.OverlapBox(transform.position + new Vector3(0, 1, 0), new Vector2(boxCollider.size.x/2, 1), 1f, aboveObject);
        if (Input.GetKeyDown(crouch) || Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching)
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.SFXSource, AudioManager.Instance.crouchingSoundEffect);
                boxCollider.size = crouchHeight;
                boxCollider.offset = colOffset;
                isCrouching = true;
            }
            else
            {
                if (crouchCollider == null)
                {
                    boxCollider.size = normalHeight;
                    boxCollider.offset = normalColOffset;
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

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + Vector2.right * offset + direction;

        //Gizmos.DrawWireSphere(origin, currentPushingRange);
        if (movingRight == false) Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.left * offset, currentPushingRange);
        else Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * offset, currentPushingRange);

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x* 1.5f);
        if (boxCollider!=null)
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 1, 0), new Vector3(boxCollider.size.x/2, 1f, 0f)) ;
    }
}
