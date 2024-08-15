using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D col;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        DeflectingShield();
    }
    void DeflectingShield()
    {
        if(transform.parent.GetComponent<Controls>().controlsAvaialble)
        {
            if (Input.GetKey(KeyCode.F))
            {
                col.enabled = true;
                //spriteRenderer.color = Color.HSVToRGB(322/360, 35/100, 100/100);
                spriteRenderer.color = Color.magenta;
            }
            else
            {
                col.enabled = false;
                spriteRenderer.color = Color.white;
            }

        }
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.TryGetComponent(out Projectile projectile))
        {            
            Vector2 direction = (projectile.parentEnemy.transform.position - collision.transform.position).normalized;
            Rigidbody2D projectileRigidbody = projectile.Rb;
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = direction*12;
            }
            projectile.gotDeflected = true;
        }
    }
}
