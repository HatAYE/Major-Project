using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] float fadeDuration; 
    [SerializeField] float waitTime;
    [SerializeField] float pauseBeforeAppearance;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }
    public IEnumerator FadeInAndOut()
    {
        yield return new WaitForSeconds(pauseBeforeAppearance);
        // Fade in
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        // Wait for a bit
        yield return new WaitForSeconds(waitTime);

        // Fade out
        yield return StartCoroutine(Fade(1, 0, fadeDuration));
    }

    IEnumerator Fade(float startOpacity, float endOpacity, float duration)
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startOpacity, endOpacity, elapsedTime / duration);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }

        // Ensure the final alpha value is set
        color.a = endOpacity;
        spriteRenderer.color = color;
    }
}
