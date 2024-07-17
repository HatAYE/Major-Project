using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compressor : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float secondsPause;
    //float time;
    //[SerializeField] float totalTime;
    [SerializeField] float distance;
    Vector3 originalPos;
    [SerializeField] bool goingUp;
    Vector3 targetPos;
    [SerializeField] bool corRunning;
    void Start()
    {
        originalPos = transform.position;
        targetPos = transform.position + new Vector3(distance, 0, 0);

        StartCoroutine(MovePlate());
    }

    // Update is called once per frame
    void Update()
    {
        if(!corRunning)
        {
            
        }
    }

    IEnumerator MovePlate()
    {
        corRunning = true;
        /*while (time < totalTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(distance, 0, 0), time / totalTime);
            yield return null;
        }
        time = 0;*/
        while (true)
        {
            if (goingUp == false)
            {
                if (Vector3.Distance(transform.position, targetPos) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, speed);
                    print("'should b moving");
                }
                else
                {
                    yield return new WaitForSeconds(secondsPause);
                    goingUp = true;
                }

            }
            else if (goingUp == true)
            {
                if (Vector2.Distance(transform.position, originalPos) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, originalPos, speed);
                    print("'owejf");
                }
                else
                {
                    yield return new WaitForSeconds(secondsPause);
                    goingUp = false;
                }
            }
            yield return null;
        }
            corRunning= false;
    }
}
