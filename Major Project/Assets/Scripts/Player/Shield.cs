using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile))
        {            
            Vector2 direction = (collision.gameObject.GetComponent<Projectile>().parentEnemy.transform.position - collision.transform.position).normalized;
            Rigidbody2D projectileRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = direction*12;
            }
            projectile.gotDeflected = true;
        }
    }
}
