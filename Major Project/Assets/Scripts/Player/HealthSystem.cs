using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int hp=0;
    public int currentHealth;

    [SerializeField] Image[] HPUI;
    [SerializeField] Sprite[] HPSprites;
    SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite= transform.GetChild(0).GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            hp++;
            currentHealth++;
        }
        if (currentHealth>hp)
        {
            currentHealth = hp;
        }

        if(currentHealth<= 0 && hp>=1)
        {
            GetComponent<Controls>().Respawn();
            currentHealth = hp;
        }

        for (int i = 0; i < HPUI.Length; i++)
        {
            if (i < currentHealth)
            {
                HPUI[i].sprite = HPSprites[0];
            }
            else HPUI[i].sprite = HPSprites[1];

            if (i<hp)
            {
                HPUI[i].enabled = true;
            }
            else HPUI[i].enabled=false;
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth-=damageAmount;
        StartCoroutine(FlashRedCoroutine());
    }
    public void Heal(int healAmount)
    {
        currentHealth+=healAmount;
    }

    IEnumerator FlashRedCoroutine()
    {
        float elapsedTime = 0f;

        // Fade to target color
        while (elapsedTime < 0.08f)
        {
            playerSprite.color = Color.Lerp(Color.white, Color.red, elapsedTime / 0.08f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        elapsedTime = 0f;

        // Fade back to original color
        while (elapsedTime < 0.3f)
        {
            playerSprite.color = Color.Lerp(Color.red, Color.white, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerSprite.color = Color.white;
        /* playerSprite.color = Color.red;
         yield return new WaitForSeconds(0.2f);
         playerSprite.color = Color.white;*/
    }
}
