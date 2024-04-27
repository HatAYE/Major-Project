using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] int healthEffect;
    HealthSystem player;
    bool tookEffect;
    void Start()
    {
        player= FindObjectOfType<HealthSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==player.gameObject)
        {
            if (!tookEffect)
            {
                player.currentHealth += healthEffect;
                Destroy(gameObject);
                tookEffect = true;
            }
            
        }
    }
}
