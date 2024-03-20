using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject parentEnemy;
    public bool gotDeflected;
    GameObject player;
    Rigidbody2D rb;
    public Rigidbody2D Rb { get => rb; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 1)
        {
            if(!gotDeflected)
            Destroy(gameObject);
        }

        if (Vector2.Distance(transform.position, player.transform.position) >20)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==parentEnemy.gameObject)
        {
            if(gotDeflected)
            parentEnemy.GetComponent<FlyingEnemy>().gotHit = true;
        }
    }
}
