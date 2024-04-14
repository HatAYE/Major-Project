using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrail : MonoBehaviour
{
    [SerializeField] Sprite[] trials;
    SpriteRenderer spriteRenderer;
    Animation animationPlayer;
    Controls pl;
    void Start()
    {
        pl = FindObjectOfType<Controls>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (GetComponent<Animation>() != null)
            animationPlayer = GetComponent<Animation>();
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
            print("index is "+ i);
            //animationPlayer.GetClip()
        }
    }
}
