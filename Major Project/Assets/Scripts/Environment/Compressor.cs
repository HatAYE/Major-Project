using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Timeline;

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
    [SerializeField] Vector2 direction;
    Controls player;
    Collider2D col;
    [SerializeField] Collider2D hardCollider;
    void Start()
    {
        originalPos = transform.position;
        targetPos = transform.position + new Vector3(distance, 0, 0);
        player=FindObjectOfType<Controls>();
        col = GetComponent<Collider2D>();
        StartCoroutine(MovePlate());

    }

    RaycastHit2D hit;
    void FixedUpdate()
    {
        /*hit= Physics2D.Raycast(originalPos, targetPos, 3f);
        print(hit.collider.gameObject.name);
        if(!goingUp)
        {
            print("goiing up");
            if(hit.collider== player.gameObject)
            {
                player.GetComponent<Rigidbody2D>().AddForce(direction);
                print("psuhed player");
            }
            
        }*/
        hardCollider.gameObject.transform.position = transform.position;
    }

    IEnumerator MovePlate()
    {
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
                    col.enabled = true;
                }
                else
                {
                    yield return new WaitForSeconds(.2f);
                    col.enabled = false;
                    yield return new WaitForSeconds(secondsPause);
                    
                    goingUp = true;
                }

            }
            else if (goingUp == true)
            {
                if (Vector2.Distance(transform.position, originalPos) >= 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, originalPos, speed);

                }
                else
                {
                    
                    yield return new WaitForSeconds(secondsPause);
                    goingUp = false;
                }
            }
            yield return null;
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Controls player))
        {
            player.GetComponent<Rigidbody2D>().AddForce(direction);
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Controls player))
        {
            player.rb.AddForce(direction);

        }
    }
    private void OnDrawGizmos()
    {
        Color color= Color.white;
        Gizmos.DrawLine(originalPos, targetPos);
    }
}
