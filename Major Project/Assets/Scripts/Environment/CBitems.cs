using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CBitems : MonoBehaviour
{
    public bool isfalling;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Conveyor belt"))
        {
            isfalling = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
