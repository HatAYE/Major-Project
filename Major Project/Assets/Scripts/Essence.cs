using UnityEngine;

public class Essence : MonoBehaviour
{
    Controls player;
    public bool collected;

    void Start()
    {
        player = FindObjectOfType<Controls>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.gameObject == collision.gameObject)
        {
            if (!collected)
            StartCoroutine(player.AddEssence());
            collected = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            //Destroy(gameObject);
        }
    }
}