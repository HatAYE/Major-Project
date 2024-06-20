using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    bool done;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&& !done)
        {
            if (GetComponent<Animation>()!=null)
            GetComponent<Animation>().Play();
            StartCoroutine(GetComponent<FadeAnimation>().FadeInAndOut());
            done = true;
        }
    }
}
