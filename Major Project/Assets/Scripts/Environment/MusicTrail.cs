using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrail : MonoBehaviour
{
    [SerializeField] Sprite[] trials;
    SpriteRenderer spriteRenderer;
    Animation animationPlayer;
    Controls pl;

    float fadeDuration = 0.4f;
    Renderer rendererComponent;
    Material material;
    void Start()
    {
        pl = FindObjectOfType<Controls>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (GetComponent<Animation>() != null)
            animationPlayer = GetComponent<Animation>();

        rendererComponent = GetComponent<Renderer>();

        if (rendererComponent != null)
        {
            material = rendererComponent.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pl.essenceCollected +1; i++)
        {
            if (i >= trials.Length)
            {
                break;
            }
            spriteRenderer.sprite = trials[i];
            //animationPlayer.GetClip()
        }
    }
    public IEnumerator VisualFadeIn()
    {
        Color color = material.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            color.a = alpha;
            material.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 1f;
        material.color = color;
    }  
    public IEnumerator VisualFadeout()
    {
        Color color = material.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            color.a = alpha;
            material.color = color;

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        color.a = 0f;
        material.color = color;
        yield return new WaitForSeconds(1f);
    } 
}
