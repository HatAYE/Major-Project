using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalPlate : MonoBehaviour
{
    ElementType type;
    Element thisElement;
    SpriteRenderer thisSpriteRenderer;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite newSprite;
    [HideInInspector] public bool solved;
    [HideInInspector] public bool gotChecked;
    void Start()
    {
        type = GetComponent<ElementType>();
        thisElement = GetComponent<ElementType>().objectElement;
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
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
                    thisSpriteRenderer.color =Color.green;
                    GetComponent<Collider2D>().enabled = false;
                    spriteRenderer.sprite = newSprite;
                    solved = true;
                }
                else
                {
                    Destroy(collision.gameObject);
                    thisSpriteRenderer.color = Color.red;
                    StartCoroutine(FadeToOriginalColor());
                }
            }
            
        }
    }

    IEnumerator FadeToOriginalColor()
    {
        yield return new WaitForSeconds(1);

        Color originalColor = thisSpriteRenderer.color;
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            thisSpriteRenderer.color = Color.Lerp(originalColor, Color.white, timer / fadeDuration);
            yield return null;
        }

        thisSpriteRenderer.color = Color.white;
    }
}
