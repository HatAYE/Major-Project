using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forklift : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int numberRequiredToUnlock;
    [SerializeField] GameObject groundPos;
    [SerializeField] GameObject liftPos;
    bool goingUp;
    bool unlockedMachine;
    void Update()
    {
        if (unlockedMachine)
        {
            StartCoroutine(moveTheFork());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Controls player))
        {
            if (LevelOneManager.scrapParts== numberRequiredToUnlock)
            {
                if (!unlockedMachine)
                {
                    LevelOneManager.scrapParts -= numberRequiredToUnlock;
                    unlockedMachine = true;
                }
            }
        }
    }

    IEnumerator moveTheFork()
    {
        if (goingUp == false)
        {
            if (Vector3.Distance(transform.position, liftPos.transform.position) >= 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, liftPos.transform.position, speed);
            }
            else
            {
                yield return new WaitForSeconds(2);
                goingUp = true;
            }

        }
        else if (goingUp == true)
        {
            if (Vector2.Distance(transform.position, groundPos.transform.position) >= 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, groundPos.transform.position, speed);
            }
            else
            {
                yield return new WaitForSeconds(2);
                goingUp = false;
            }
        }
    }
}
