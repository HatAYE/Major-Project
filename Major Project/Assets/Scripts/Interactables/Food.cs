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
        if (collision.gameObject.TryGetComponent(out Controls pl))
        {
            if (!tookEffect)
            {
                player.Heal(healthEffect);
                StartCoroutine(pl.AddEssence());
                Destroy(gameObject);
                tookEffect = true;
            }
            
        }
    }
}
