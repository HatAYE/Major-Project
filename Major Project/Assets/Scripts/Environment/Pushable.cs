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
    [SerializeField] Material outlineMaterial;
    Material originalMaterial;

    public bool conditionalOnSize;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        player= FindObjectOfType<Controls>();
        initialMass= rb.mass;
        originalPosition = transform.position;
        originalMaterial= GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHeld)
        {
            GetComponent<SpriteRenderer>().material = originalMaterial;
            if (rb.velocity == Vector2.zero)
                rb.mass = initialMass;
        }
        else
        {
            if (outlineMaterial != null)
                GetComponent<SpriteRenderer>().material = outlineMaterial;
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
