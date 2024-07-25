using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Timeline;

public class Compressor : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float secondsPause;
    [SerializeField] Vector3 platformDirection;
    Vector3 originalPos;
    bool goingUp;
    Vector3 targetPos;
    [SerializeField] Vector2 playerForceDirection;
    Collider2D col;
    [SerializeField] Collider2D hardCollider;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        originalPos = transform.position;
        targetPos = transform.position + platformDirection;
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        StartCoroutine(MovePlate());

    }

    RaycastHit2D hit;
    void FixedUpdate()
    {
        hardCollider.gameObject.transform.position = transform.position;
    }

    IEnumerator MovePlate()
    {
        while (true)
        {
            if (goingUp == false)
            {
                if (Vector3.Distance(transform.position, targetPos) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
                    col.enabled = true;
                }
                else
                {
                    yield return new WaitForSeconds(.2f);
                    col.enabled = false;
                    yield return new WaitForSeconds(secondsPause);
                    StartCoroutine(FadeColor(Color.blue, 2));
                    goingUp = true;
                }
            }
            else if (goingUp == true)
            {
                if (Vector2.Distance(transform.position, originalPos) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, originalPos, speed);

                }
                else
                {
                    yield return new WaitForSeconds(secondsPause);
                    StartCoroutine(FadeColor(Color.white, 0.1f));
                    goingUp = false;
                }
            }
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Controls player))
        {
            player.rb.AddForce(playerForceDirection);
            StartCoroutine(DisablePlayerControlsTemporarily(player));
        }
    }
    IEnumerator DisablePlayerControlsTemporarily(Controls player)
    {
        player.controlsAvaialble = false;
        yield return new WaitForSeconds(0.5f); 
        player.controlsAvaialble = true;
    }
    IEnumerator FadeColor(Color goalColor, float duration)
    {
        Color originalColor = spriteRenderer.color;
        float fadeDuration = duration;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(originalColor, goalColor, timer / fadeDuration);
            yield return null;
        }

        spriteRenderer.color = goalColor;
    }
    private void OnDrawGizmos()
    {
        Color color= Color.white;
        Gizmos.DrawLine(originalPos, targetPos);
    }
}
