using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;
using VInspector;

public class Pushable : MonoBehaviour
{
    public float massWhenHeld;
    [HideInInspector] public bool gotLetGo;
    float initialMass;
    [HideInInspector] public bool beingHeld;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector3 originalPosition;
    Controls player;

    public bool conditionalOnSize;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        player= FindObjectOfType<Controls>();
        initialMass= rb.mass;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHeld)
        {
            if (rb.velocity== Vector2.zero)
            rb.mass = initialMass;
        }
        if (gotLetGo)
        {
            if (rb.velocity != Vector2.zero)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
            }
            else
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
                gotLetGo = false;
            }

        }
    }
}
