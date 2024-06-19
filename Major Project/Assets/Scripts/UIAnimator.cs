using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] float speed;
    int spriteIndex;
    void Start()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        while (true)
        {
            print("ds");
            yield return new WaitForSeconds(speed);
            spriteIndex++;
            if (spriteIndex <= sprites.Count)
                spriteIndex = 0;
            else
                image.sprite = sprites[spriteIndex];
        }
    }
}
