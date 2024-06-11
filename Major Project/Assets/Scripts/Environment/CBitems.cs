using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBitems : MonoBehaviour
{
    [HideInInspector] public bool isfalling;
    public ConveyerBelt conveyer;
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.name.StartsWith("Conveyor belt"))
        {
            isfalling = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            conveyer.items.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
