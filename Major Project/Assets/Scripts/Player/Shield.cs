using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile))
        {            
            Vector2 direction = projectile.parentEnemy.transform.position + new Vector3 (0,3.5f,0);
            Rigidbody2D projectileRigidbody = projectile.Rb;
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = direction;
            }
            projectile.gotDeflected = true;
        }
    }
}
