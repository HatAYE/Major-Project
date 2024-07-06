using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBreak : MonoBehaviour
{
    bool coroutineRunning;
    [SerializeField] bool resetable;
    [SerializeField] float flashDuration = 3f; // Duration to flash
    [SerializeField] float dropDistance = 50f; // Distance to drop
    [SerializeField] float dropTime = 2f; // Time to drop
    [SerializeField] float resetTime = 5f; // Time to reset

    private SpriteRenderer spriteRenderer;
    private Vector2 originalPosition;
    private Color originalColor;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        originalColor = spriteRenderer.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!coroutineRunning)
                StartCoroutine(BreakTimer());
        }
    }

    IEnumerator BreakTimer()
    {
        coroutineRunning = true;
        // Flashing sprite
        float timer = flashDuration;
        while (timer > 0)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.PingPong(Time.time * 2, 1));
            timer -= Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;

        // Drop the object
        Vector2 targetPosition = originalPosition - new Vector2(0, dropDistance);
        float dropTimer = 0;
        while (dropTimer < dropTime)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, dropTimer / dropTime);
            dropTimer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        if(resetable)
        {
            yield return new WaitForSeconds(resetTime);

            transform.position = originalPosition;

            spriteRenderer.color = originalColor;
            coroutineRunning = false;
        }
        if(!resetable)
            Destroy(gameObject);
        
    }
}
