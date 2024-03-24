using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaDetection : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    BoxCollider2D col;
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        enemy.areaDetector = gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            enemy.playerInRadius=true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            enemy.playerInRadius = false;
    }
    
    void OnDrawGizmos()
    {
        if (col == null)
            return;
        Bounds bounds = col.bounds;

        float size = Mathf.Max(bounds.size.x, bounds.size.y);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, new Vector3(size, size, 0));
    }
}
