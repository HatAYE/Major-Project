using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBitems : MonoBehaviour
{
    [HideInInspector] public bool isfalling;
    public ConveyerBelt conveyer;
    [HideInInspector] public bool playerOnItem;
    [SerializeField] float timer;
    [SerializeField] float maxTimer=100;
    private void Update()
    {
        if (timer < maxTimer)
        {
            timer += 0.05f;
        }
        else Destroy(gameObject);
    }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerOnItem = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnItem = false;
        }
    }
}

