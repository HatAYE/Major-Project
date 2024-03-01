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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth>hp)
        {
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
}
