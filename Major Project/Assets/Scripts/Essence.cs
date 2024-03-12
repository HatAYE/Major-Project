using UnityEngine;

public class Essence : MonoBehaviour
{
    Controls player;

    void Start()
    {
        player = FindObjectOfType<Controls>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.gameObject == collision.gameObject)
        {
            player.essenceCollected++;
            Destroy(gameObject);
        }
    }
}