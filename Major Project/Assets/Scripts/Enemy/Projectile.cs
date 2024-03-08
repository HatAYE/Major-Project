using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject parentEnemy;
    public bool gotDeflected;
    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float distanceThreshold = 0.1f; // Adjust the threshold as needed

        if (Vector2.Distance(transform.position, player.transform.position) < distanceThreshold)
        {
            if(!gotDeflected)
            Destroy(gameObject);
        }
    }
}
