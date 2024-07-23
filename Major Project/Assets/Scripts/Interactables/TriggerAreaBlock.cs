using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BlockAreas areas))
        {
            //GetComponent<Collider2D>().isTrigger = false;
            areas.BlockArea();
        }
    }
}
