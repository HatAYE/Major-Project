using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrail : MonoBehaviour
{
    [SerializeField] GameObject[] trails;
    SpriteRenderer spriteRenderer;
    Controls pl;

    float fadeDuration = 1f;
    void Start()
    {
        pl = FindObjectOfType<Controls>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pl.onEssenceCollection += UpdateMusicTrail;

    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i < pl.essenceCollected +1; i++)
        {
            if (i >= trails.Length)
            {
                break;
            }
            spriteRenderer.sprite = trails[i].GetComponent<Sprite>();
        }*/
    }
    int trailIndex;
    void UpdateMusicTrail()
    {
        if (trailIndex < trails.Length)
        {
            if (trails[trailIndex] != null)
            {
                StartCoroutine(VisualFadeIn(trails[trailIndex].GetComponent<SpriteRenderer>()));
                trailIndex++;
            }
        }
        else
        {
            pl.onEssenceCollection -= UpdateMusicTrail;
        }
    }

    public IEnumerator FadeInMusicTrails()
    {
        for (int i = 0; i < trailIndex; i++)
        {
            StartCoroutine(VisualFadeIn(trails[i].GetComponent<SpriteRenderer>()));
        }
        yield return null;
    }
    public IEnumerator FadeOutMusicTrails()
    {
        for (int i = 0; i < trailIndex; i++)
        {
            StartCoroutine(VisualFadeout(trails[i].GetComponent<SpriteRenderer>()));
        }
        yield return null;
    }
    public IEnumerator VisualFadeIn(SpriteRenderer spriteRenderer)
    {
        Color color = spriteRenderer.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            color.a = alpha;
            spriteRenderer.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 1f;
        spriteRenderer.color = color;
    }  
    public IEnumerator VisualFadeout(SpriteRenderer spriteRenderer)
    {
        Color color = spriteRenderer.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            color.a = alpha;
            spriteRenderer.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 0f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(1f);
    } 
}
