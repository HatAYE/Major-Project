using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Essence : MonoBehaviour
{
    Controls player;
    void Start()
    {
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        player= pl.GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.essenceCollected++;
            Destroy(gameObject);
        }

    }
}
