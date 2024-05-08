using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    float fireflyRadius = 1.5f;
    [SerializeField] float fireflySpeed = 0.7f;
    float playerRadius=1.5f;

    Vector3 initialPosition;
    Vector3 targetPosition;

    float timer;
    [SerializeField] float timeBetweenTargets = 1f;
    bool followingPlayer;
    bool inPlayerRadius;
    Controls player;
    void Start()
    {
        initialPosition = transform.position;
        player= FindObjectOfType<Controls>();
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (gameObject.GetComponent<Essence>().collected==true)
        followingPlayer = true;

        if (followingPlayer)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 1f && !inPlayerRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, fireflySpeed * Time.deltaTime *5);
                inPlayerRadius = false;
                SetRandomTargetPosition();
            }
            else
            {
                inPlayerRadius = true;
            }

            if (inPlayerRadius)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 1f)
                {
                    if (timer <= 0)
                    {
                        timer = timeBetweenTargets;
                        SetRandomTargetPosition();
                    }
                }
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, fireflySpeed * Time.deltaTime);
                    timer -= Time.deltaTime;

                if (Vector3.Distance(transform.position, player.transform.position) > 1f)
                {
                    inPlayerRadius= false;
                }
            }
            
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                if (timer <= 0)
                {
                    timer = timeBetweenTargets;
                    SetRandomTargetPosition();
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fireflySpeed * Time.deltaTime);
            timer -= Time.deltaTime;
        }
    }

    void SetRandomTargetPosition()
    {
        if (followingPlayer)
        {
            targetPosition = (Vector2)player.transform.position + Random.insideUnitCircle * playerRadius;
        }
        else
        {
            targetPosition = (Vector2)initialPosition + Random.insideUnitCircle * fireflyRadius;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (!followingPlayer)
        Gizmos.DrawWireSphere(initialPosition, fireflyRadius);
         
        if (followingPlayer && player.transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, playerRadius);
        }
    }
}
