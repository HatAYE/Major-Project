using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] int jumpCount;
    void Update()
    {
        if (jumpCount == 3)
        {
            //play break animation
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered: " + collision.gameObject.name);
        if (collision.isTrigger)
        {
            if (collision.TryGetComponent(out Controls player))
            {
                if (player.GetComponent<ShrinkingAndEnlarging>() != null)
                {
                    if (player.GetComponent<ShrinkingAndEnlarging>().currentSize == player.GetComponent<ShrinkingAndEnlarging>().largeSize)
                        jumpCount++;
                }
            }
        }
    }
}
