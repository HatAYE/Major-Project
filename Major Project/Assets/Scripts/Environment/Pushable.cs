using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public float massWhenHeld;
    float initialMass;
    [HideInInspector] public bool beingHeld;
    [HideInInspector] public Rigidbody2D rb;

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        initialMass= rb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingHeld)
        {
            if (rb.velocity== Vector2.zero)
            rb.mass = initialMass;
        }
    }
}
