using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Projectile"))
        {            
            Vector2 direction = (collision.gameObject.GetComponent<Projectile>().parentEnemy.transform.position - collision.transform.position).normalized;
            Rigidbody2D projectileRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = direction*12;
            }
            collision.gameObject.GetComponent<Projectile>().gotDeflected = true;
        }
    }
}
