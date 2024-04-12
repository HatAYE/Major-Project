using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] int jumpCount;
    void Update()
    {
        if (jumpCount==3)
        {
            //play break animation
            //Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controls player))
        {
            if(player.gameObject.TryGetComponent(out BoxCollider2D collider))
            {
                if (collider.isTrigger)
                {
                    if (player.GetComponent<ShrinkingAndEnlarging>().currentSize == player.GetComponent<ShrinkingAndEnlarging>().largeSize)
                        jumpCount++;
                }
                
            }
            
        }
    }
}
