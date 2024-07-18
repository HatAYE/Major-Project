using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalPlate : MonoBehaviour
{
    ElementType type;
    Element thisElement;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        type = GetComponent<ElementType>();
        thisElement = GetComponent<ElementType>().objectElement;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ElementType elementType))
        {
            if(collision.gameObject.GetComponent<Pushable>().beingHeld== false)
            {
                if (elementType.objectElement == thisElement)
                {
                    Destroy(collision.gameObject);
                    spriteRenderer.color =Color.green;
                    //deactivate collider
                    GetComponent<Collider2D>().enabled = false;
                }
                else
                {
                    Destroy(collision.gameObject);
                    spriteRenderer.color = Color.red;
                    StartCoroutine(FadeToOriginalColor());
                }
            }
            
        }
    }

    IEnumerator FadeToOriginalColor()
    {
        yield return new WaitForSeconds(1);

        Color originalColor = spriteRenderer.color;
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(originalColor, Color.white, timer / fadeDuration);
            yield return null;
        }

        spriteRenderer.color = Color.white;
    }
}
